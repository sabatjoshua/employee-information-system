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
        var departmentRepository = new Mock<IDepartmentRepository>();
        var positionRepository = new Mock<IPositionRepository>();

        var employeeId = Guid.NewGuid();

        var department = new Department
        {
            Name = "Human Resources",
            StatusCode = StatusCodes.Active,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        var position = new Position
        {
            Name = "Senior Software Engineer",
            DepartmentId = department.Id,
            StatusCode = StatusCodes.Active,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow
        };

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
            DepartmentId = department.Id,
            PositionId = position.Id,
            CreatedBy = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            StatusCode = StatusCodes.Active
        };

        employeeRepository
            .Setup(x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        departmentRepository
            .Setup(x => x.GetByIdAsync(
                department.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(department);

        positionRepository
            .Setup(x => x.GetByIdAsync(
                position.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(position);

        var handler = new GetEmployeeByIdHandler(
            employeeRepository.Object,
            departmentRepository.Object,
            positionRepository.Object);

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

        Assert.Equal(department.Id, result.DepartmentId);
        Assert.Equal("Human Resources", result.DepartmentName);

        Assert.Equal(position.Id, result.PositionId);
        Assert.Equal("Senior Software Engineer", result.PositionName);

        employeeRepository.Verify(
            x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        departmentRepository.Verify(
            x => x.GetByIdAsync(
                department.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        positionRepository.Verify(
            x => x.GetByIdAsync(
                position.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();
        var departmentRepository = new Mock<IDepartmentRepository>();
        var positionRepository = new Mock<IPositionRepository>();

        var employeeId = Guid.NewGuid();

        employeeRepository
            .Setup(x => x.GetByIdAsync(
                employeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        var handler = new GetEmployeeByIdHandler(
            employeeRepository.Object,
            departmentRepository.Object,
            positionRepository.Object);

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

        departmentRepository.Verify(
            x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        positionRepository.Verify(
            x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}