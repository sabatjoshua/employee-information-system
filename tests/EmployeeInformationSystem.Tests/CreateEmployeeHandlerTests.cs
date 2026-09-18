using EmployeeInformationSystem.Application.Common.Interfaces;
using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Common.Interfaces.Security;
using EmployeeInformationSystem.Application.Features.Employees;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using Moq;

namespace EmployeeInformationSystem.Tests;

public class CreateEmployeeHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateEmployeeAndHistory_WhenCommandIsValid()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        var employeeHistoryRepository = new Mock<IEmployeeHistoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var currentUserService = new Mock<ICurrentUserService>();

        var currentUserId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var positionId = Guid.NewGuid();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(currentUserId);

        Employee? createdEmployee = null;
        EmployeeHistory? createdHistory = null;

        employeeRepository
            .Setup(x => x.AddAsync(
                It.IsAny<Employee>(),
                It.IsAny<CancellationToken>()))
            .Callback<Employee, CancellationToken>(
                (employee, _) => createdEmployee = employee)
            .Returns(Task.CompletedTask);

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

        var handler = new CreateEmployeeHandler(
            employeeRepository.Object,
            unitOfWork.Object,
            employeeHistoryRepository.Object,
            currentUserService.Object);

        var command = new CreateEmployeeCommand(
            EmployeeNo: "EMP001",
            FirstName: "Joshua",
            MiddleName: null,
            LastName: "Sabat",
            GenderCode: "M",
            BirthDate: new DateTimeOffset(
                1984, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Email: "joshua@example.com",
            MobileNo: "91234567",
            HireDate: new DateTimeOffset(
                2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DepartmentId: departmentId,
            PositionId: positionId);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(createdEmployee);
        Assert.NotNull(createdHistory);

        Assert.Equal("EMP001", createdEmployee.EmployeeNo);
        Assert.Equal("Joshua", createdEmployee.FirstName);
        Assert.Equal("Sabat", createdEmployee.LastName);
        Assert.Equal(currentUserId, createdEmployee.CreatedBy);
        Assert.Equal(StatusCodes.Active, createdEmployee.StatusCode);

        Assert.Equal(createdEmployee.Id, createdHistory.EmployeeId);
        Assert.Equal(ActionTypeCodes.Insert, createdHistory.ActionTypeCode);
        Assert.Equal(currentUserId, createdHistory.ActionBy);

        Assert.Equal(createdEmployee.Id, result.Id);
        Assert.Equal("EMP001", result.EmployeeNo);
        Assert.Equal("Joshua", result.FirstName);
        Assert.Equal("Sabat", result.LastName);

        employeeRepository.Verify(
            x => x.AddAsync(
                It.IsAny<Employee>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

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
}