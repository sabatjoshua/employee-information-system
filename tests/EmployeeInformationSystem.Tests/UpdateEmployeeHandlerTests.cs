using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Application.Features.Employees;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using Moq;

namespace EmployeeInformationSystem.Tests;

public class UpdateEmployeeHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateEmployeeAndCreateHistory_WhenEmployeeExists()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        var employeeHistoryRepository = new Mock<IEmployeeHistoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var currentUserService = new Mock<ICurrentUserService>();

        var employeeId = Guid.NewGuid();
        var currentUserId = Guid.NewGuid();
        var oldDepartmentId = Guid.NewGuid();
        var oldPositionId = Guid.NewGuid();
        var newDepartmentId = Guid.NewGuid();
        var newPositionId = Guid.NewGuid();

        var employee = new Employee
        {
            EmployeeNo = "EMP001",
            FirstName = "Old",
            MiddleName = null,
            LastName = "Name",
            GenderCode = "M",
            BirthDate = new DateTimeOffset(
                1984, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Email = "old@example.com",
            MobileNo = "11111111",
            HireDate = new DateTimeOffset(
                2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DepartmentId = oldDepartmentId,
            PositionId = oldPositionId,
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

        var handler = new UpdateEmployeeHandler(
            employeeRepository.Object,
            employeeHistoryRepository.Object,
            unitOfWork.Object,
            currentUserService.Object);

        var command = new UpdateEmployeeCommand(
            EmployeeId: employeeId,
            EmployeeNo: "EMP002",
            FirstName: "Joshua",
            MiddleName: "Plaza",
            LastName: "Sabat",
            GenderCode: "M",
            BirthDate: new DateTimeOffset(
                1984, 2, 1, 0, 0, 0, TimeSpan.Zero),
            Email: "joshua@example.com",
            MobileNo: "91234567",
            HireDate: new DateTimeOffset(
                2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DepartmentId: newDepartmentId,
            PositionId: newPositionId);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(createdHistory);

        Assert.Equal("EMP002", employee.EmployeeNo);
        Assert.Equal("Joshua", employee.FirstName);
        Assert.Equal("Plaza", employee.MiddleName);
        Assert.Equal("Sabat", employee.LastName);
        Assert.Equal("joshua@example.com", employee.Email);
        Assert.Equal(newDepartmentId, employee.DepartmentId);
        Assert.Equal(newPositionId, employee.PositionId);

        Assert.Equal(currentUserId, employee.UpdatedBy);
        Assert.NotNull(employee.UpdatedAt);

        Assert.Equal(employee.Id, createdHistory.EmployeeId);
        Assert.Equal("EMP002", createdHistory.EmployeeNo);
        Assert.Equal("Joshua", createdHistory.FirstName);
        Assert.Equal(ActionTypeCodes.Update, createdHistory.ActionTypeCode);
        Assert.Equal(currentUserId, createdHistory.ActionBy);

        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal("EMP002", result.EmployeeNo);
        Assert.Equal("Joshua", result.FirstName);
        Assert.Equal("Plaza", result.MiddleName);
        Assert.Equal("Sabat", result.LastName);
        Assert.Equal(StatusCodes.Active, result.StatusCode);

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

        var handler = new UpdateEmployeeHandler(
            employeeRepository.Object,
            employeeHistoryRepository.Object,
            unitOfWork.Object,
            currentUserService.Object);

        var command = new UpdateEmployeeCommand(
            EmployeeId: employeeId,
            EmployeeNo: "EMP002",
            FirstName: "Joshua",
            MiddleName: null,
            LastName: "Sabat",
            GenderCode: "M",
            BirthDate: DateTimeOffset.UtcNow.AddYears(-30),
            Email: "joshua@example.com",
            MobileNo: "91234567",
            HireDate: DateTimeOffset.UtcNow,
            DepartmentId: Guid.NewGuid(),
            PositionId: Guid.NewGuid());

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