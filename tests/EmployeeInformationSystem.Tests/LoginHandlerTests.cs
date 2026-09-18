using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Application.Features.Authentication;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using Moq;

namespace EmployeeInformationSystem.Tests;

public class LoginHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTokenAndResetFailedAttempts_WhenCredentialsAreValid()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userHistoryRepository = new Mock<IUserHistoryRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        var employeeId = Guid.NewGuid();

        var user = new User
        {
            EmployeeId = employeeId,
            UserName = "super",
            PasswordHash = "hashed-password",
            LastLogin = null,
            FailedLoginAttempt = 3,
            PasswordChangedDate = DateTimeOffset.UtcNow.AddDays(-30),
            MustChangePassword = false,
            IsLocked = false,
            StatusCode = StatusCodes.Active,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.AddYears(-1)
        };

        userRepository
            .Setup(x => x.GetByUserNameAsync(
                "super",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(x => x.Verify(
                "correct-password",
                user.PasswordHash))
            .Returns(true);

        jwtTokenGenerator
            .Setup(x => x.GenerateToken(
                user.Id,
                user.EmployeeId,
                user.UserName))
            .Returns("test-jwt-token");

        UserHistory? createdHistory = null;

        userHistoryRepository
            .Setup(x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()))
            .Callback<UserHistory, CancellationToken>(
                (history, _) => createdHistory = history)
            .Returns(Task.CompletedTask);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new LoginHandler(
            userRepository.Object,
            userHistoryRepository.Object,
            passwordHasher.Object,
            unitOfWork.Object,
            jwtTokenGenerator.Object);

        var command = new LoginCommand(
            UserName: "super",
            Password: "correct-password");

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(createdHistory);

        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(employeeId, result.EmployeeId);
        Assert.Equal("super", result.UserName);
        Assert.Equal("test-jwt-token", result.Token);

        Assert.Equal(0, user.FailedLoginAttempt);
        Assert.NotNull(user.LastLogin);

        Assert.Equal(user.Id, createdHistory.UserId);
        Assert.Equal(ActionTypeCodes.Login, createdHistory.ActionTypeCode);
        Assert.Equal(employeeId, createdHistory.ActionBy);
        Assert.Equal(0, createdHistory.FailedLoginAttempt);
        Assert.Equal(user.LastLogin, createdHistory.LastLogin);

        jwtTokenGenerator.Verify(
            x => x.GenerateToken(
                user.Id,
                employeeId,
                "super"),
            Times.Once);

        userHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldIncrementFailedAttemptsAndReturnNull_WhenPasswordIsInvalid()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userHistoryRepository = new Mock<IUserHistoryRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        var user = new User
        {
            EmployeeId = Guid.NewGuid(),
            UserName = "super",
            PasswordHash = "hashed-password",
            LastLogin = null,
            FailedLoginAttempt = 2,
            PasswordChangedDate = DateTimeOffset.UtcNow.AddDays(-30),
            MustChangePassword = false,
            IsLocked = false,
            StatusCode = StatusCodes.Active,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.AddYears(-1)
        };

        userRepository
            .Setup(x => x.GetByUserNameAsync(
                "super",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(x => x.Verify(
                "wrong-password",
                user.PasswordHash))
            .Returns(false);

        UserHistory? createdHistory = null;

        userHistoryRepository
            .Setup(x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()))
            .Callback<UserHistory, CancellationToken>(
                (history, _) => createdHistory = history)
            .Returns(Task.CompletedTask);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new LoginHandler(
            userRepository.Object,
            userHistoryRepository.Object,
            passwordHasher.Object,
            unitOfWork.Object,
            jwtTokenGenerator.Object);

        var command = new LoginCommand(
            UserName: "super",
            Password: "wrong-password");

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Null(result);
        Assert.NotNull(createdHistory);

        Assert.Equal(3, user.FailedLoginAttempt);
        Assert.False(user.IsLocked);

        Assert.Equal(3, createdHistory.FailedLoginAttempt);
        Assert.False(createdHistory.IsLocked);
        Assert.Equal(ActionTypeCodes.Login, createdHistory.ActionTypeCode);

        jwtTokenGenerator.Verify(
            x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>()),
            Times.Never);

        userHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldLockUser_WhenFailedAttemptsReachFive()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userHistoryRepository = new Mock<IUserHistoryRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        var user = new User
        {
            EmployeeId = Guid.NewGuid(),
            UserName = "super",
            PasswordHash = "hashed-password",
            LastLogin = null,
            FailedLoginAttempt = 4,
            PasswordChangedDate = DateTimeOffset.UtcNow.AddDays(-30),
            MustChangePassword = false,
            IsLocked = false,
            StatusCode = StatusCodes.Active,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.AddYears(-1)
        };

        userRepository
            .Setup(x => x.GetByUserNameAsync(
                "super",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(x => x.Verify(
                "wrong-password",
                user.PasswordHash))
            .Returns(false);

        UserHistory? createdHistory = null;

        userHistoryRepository
            .Setup(x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()))
            .Callback<UserHistory, CancellationToken>(
                (history, _) => createdHistory = history)
            .Returns(Task.CompletedTask);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new LoginHandler(
            userRepository.Object,
            userHistoryRepository.Object,
            passwordHasher.Object,
            unitOfWork.Object,
            jwtTokenGenerator.Object);

        var command = new LoginCommand(
            UserName: "super",
            Password: "wrong-password");

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Null(result);
        Assert.NotNull(createdHistory);

        Assert.Equal(5, user.FailedLoginAttempt);
        Assert.True(user.IsLocked);

        Assert.Equal(5, createdHistory.FailedLoginAttempt);
        Assert.True(createdHistory.IsLocked);
        Assert.Equal(ActionTypeCodes.Login, createdHistory.ActionTypeCode);

        jwtTokenGenerator.Verify(
            x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>()),
            Times.Never);

        userHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnNullImmediately_WhenUserIsAlreadyLocked()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userHistoryRepository = new Mock<IUserHistoryRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        var user = new User
        {
            EmployeeId = Guid.NewGuid(),
            UserName = "super",
            PasswordHash = "hashed-password",
            LastLogin = null,
            FailedLoginAttempt = 5,
            PasswordChangedDate = DateTimeOffset.UtcNow.AddDays(-30),
            MustChangePassword = false,
            IsLocked = true,
            StatusCode = StatusCodes.Active,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.AddYears(-1)
        };

        userRepository
            .Setup(x => x.GetByUserNameAsync(
                "super",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new LoginHandler(
            userRepository.Object,
            userHistoryRepository.Object,
            passwordHasher.Object,
            unitOfWork.Object,
            jwtTokenGenerator.Object);

        var command = new LoginCommand(
            UserName: "super",
            Password: "correct-password");

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Null(result);

        passwordHasher.Verify(
            x => x.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        jwtTokenGenerator.Verify(
            x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>()),
            Times.Never);

        userHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldReturnNullImmediately_WhenUserIsInactive()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userHistoryRepository = new Mock<IUserHistoryRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        var user = new User
        {
            EmployeeId = Guid.NewGuid(),
            UserName = "super",
            PasswordHash = "hashed-password",
            LastLogin = null,
            FailedLoginAttempt = 0,
            PasswordChangedDate = DateTimeOffset.UtcNow.AddDays(-30),
            MustChangePassword = false,
            IsLocked = false,
            StatusCode = StatusCodes.Inactive,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.AddYears(-1)
        };

        userRepository
            .Setup(x => x.GetByUserNameAsync(
                "super",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new LoginHandler(
            userRepository.Object,
            userHistoryRepository.Object,
            passwordHasher.Object,
            unitOfWork.Object,
            jwtTokenGenerator.Object);

        var command = new LoginCommand(
            UserName: "super",
            Password: "correct-password");

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Null(result);

        passwordHasher.Verify(
            x => x.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        jwtTokenGenerator.Verify(
            x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>()),
            Times.Never);

        userHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<UserHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}