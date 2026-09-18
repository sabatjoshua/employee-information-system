using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Features.Employees;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using Moq;

namespace EmployeeInformationSystem.Tests;

public class GetEmployeeByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        var employeeId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var positionId = Guid.NewGuid();

        var employee = new Employee
        {
            EmployeeNo = "EMP001",
            FirstName = "Joshua",
            MiddleName = "Plaza",
            LastName = "Sabat",
            GenderCode = "M",
            BirthDate = new DateTimeOffset(
                1984, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Email = "joshua@example.com",
            MobileNo = "91234567",
            HireDate = new DateTimeOffset(
                2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            DepartmentId = departmentId,
            PositionId = positionId,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            StatusCode = StatusCodes.Active
        };

        employeeRepository
            .Setup(x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        var handler = new GetEmployeeByIdHandler(
            employeeRepository.Object);

        var query = new GetEmployeeByIdQuery(employeeId);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal("EMP001", result.EmployeeNo);
        Assert.Equal("Joshua", result.FirstName);
        Assert.Equal("Plaza", result.MiddleName);
        Assert.Equal("Sabat", result.LastName);
        Assert.Equal("M", result.GenderCode);
        Assert.Equal(employee.BirthDate, result.BirthDate);
        Assert.Equal("joshua@example.com", result.Email);
        Assert.Equal("91234567", result.MobileNo);
        Assert.Equal(employee.HireDate, result.HireDate);
        Assert.Equal(departmentId, result.DepartmentId);
        Assert.Equal(positionId, result.PositionId);

        employeeRepository.Verify(
            x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        var employeeId = Guid.NewGuid();

        employeeRepository
            .Setup(x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        var handler = new GetEmployeeByIdHandler(
            employeeRepository.Object);

        var query = new GetEmployeeByIdQuery(employeeId);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.Null(result);

        employeeRepository.Verify(
            x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}