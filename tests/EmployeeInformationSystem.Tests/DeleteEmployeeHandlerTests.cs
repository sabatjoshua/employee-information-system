using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Application.Features.Employees;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using Moq;

namespace EmployeeInformationSystem.Tests;

public class DeleteEmployeeHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeactivateEmployeeAndCreateHistory_WhenEmployeeExists()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        var employeeHistoryRepository = new Mock<IEmployeeHistoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var currentUserService = new Mock<ICurrentUserService>();

        var employeeId = Guid.NewGuid();
        var currentUserId = Guid.NewGuid();

        var employee = new Employee
        {
            EmployeeNo = "EMP001",
            FirstName = "Joshua",
            MiddleName = null,
            LastName = "Sabat",
            GenderCode = "M",
            BirthDate = new DateTimeOffset(
                1984, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Email = "joshua@example.com",
            MobileNo = "91234567",
            HireDate = new DateTimeOffset(
                2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DepartmentId = Guid.NewGuid(),
            PositionId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow.AddYears(-1),
            StatusCode = StatusCodes.Active
        };

        employeeRepository
            .Setup(x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        currentUserService
            .Setup(x => x.UserId)
            .Returns(currentUserId);

        EmployeeHistory? createdHistory = null;

        employeeHistoryRepository
            .Setup(x => x.AddAsync(
                It.IsAny<EmployeeHistory>(),
                It.IsAny<CancellationToken>()))
            .Callback<EmployeeHistory, CancellationToken>(
                (history, _) => createdHistory = history)
            .Returns(Task.CompletedTask);

        unitOfWork
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteEmployeeHandler(
            employeeRepository.Object,
            employeeHistoryRepository.Object,
            unitOfWork.Object,
            currentUserService.Object);

        var command = new DeleteEmployeeCommand(employeeId);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(createdHistory);

        Assert.Equal(StatusCodes.Inactive, employee.StatusCode);

        Assert.Equal(employee.Id, createdHistory.EmployeeId);
        Assert.Equal(StatusCodes.Inactive, createdHistory.StatusCode);
        Assert.Equal(ActionTypeCodes.Delete, createdHistory.ActionTypeCode);
        Assert.Equal(currentUserId, createdHistory.ActionBy);

        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal(StatusCodes.Inactive, result.StatusCode);

        employeeHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<EmployeeHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        var employeeHistoryRepository = new Mock<IEmployeeHistoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var currentUserService = new Mock<ICurrentUserService>();

        var employeeId = Guid.NewGuid();

        employeeRepository
            .Setup(x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        var handler = new DeleteEmployeeHandler(
            employeeRepository.Object,
            employeeHistoryRepository.Object,
            unitOfWork.Object,
            currentUserService.Object);

        var command = new DeleteEmployeeCommand(employeeId);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Null(result);

        employeeHistoryRepository.Verify(
            x => x.AddAsync(
                It.IsAny<EmployeeHistory>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}