using EmployeeInformationSystem.Application.Common.Interfaces.Repositories;
using EmployeeInformationSystem.Application.Features.Employees;
using EmployeeInformationSystem.Domain.Constants;
using EmployeeInformationSystem.Domain.Entities;
using Moq;

namespace EmployeeInformationSystem.Tests;

public class GetEmployeesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPagedEmployees_WhenEmployeesExist()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        var departmentId = Guid.NewGuid();
        var positionId = Guid.NewGuid();

        var employees = new List<Employee>
        {
            new Employee
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
            },
            new Employee
            {
                EmployeeNo = "EMP002",
                FirstName = "John",
                MiddleName = null,
                LastName = "Doe",
                GenderCode = "M",
                BirthDate = new DateTimeOffset(
                    1990, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Email = "john@example.com",
                MobileNo = "92345678",
                HireDate = new DateTimeOffset(
                    2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
                DepartmentId = departmentId,
                PositionId = positionId,
                CreatedBy = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow,
                StatusCode = StatusCodes.Active
            }
        };

        employeeRepository
            .Setup(x => x.GetPagedAsync(
                1,
                2,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(employees);

        employeeRepository
            .Setup(x => x.CountAsync(
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);

        var handler = new GetEmployeesHandler(
            employeeRepository.Object);

        var query = new GetEmployeesQuery(
            PageNumber: 1,
            PageSize: 2,
            Search: null);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(3, result.TotalPages);

        Assert.Equal("EMP001", result.Items[0].EmployeeNo);
        Assert.Equal("Joshua", result.Items[0].FirstName);
        Assert.Equal("Sabat", result.Items[0].LastName);

        Assert.Equal("EMP002", result.Items[1].EmployeeNo);
        Assert.Equal("John", result.Items[1].FirstName);
        Assert.Equal("Doe", result.Items[1].LastName);

        employeeRepository.Verify(
            x => x.GetPagedAsync(
                1,
                2,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);

        employeeRepository.Verify(
            x => x.CountAsync(
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldUseDefaultPagination_WhenPageValuesAreInvalid()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        employeeRepository
            .Setup(x => x.GetPagedAsync(
                1,
                10,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Employee>());

        employeeRepository
            .Setup(x => x.CountAsync(
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var handler = new GetEmployeesHandler(
            employeeRepository.Object);

        var query = new GetEmployeesQuery(
            PageNumber: 0,
            PageSize: 0,
            Search: null);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);

        employeeRepository.Verify(
            x => x.GetPagedAsync(
                1,
                10,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldLimitPageSizeTo100_WhenPageSizeExceedsMaximum()
    {
        // Arrange
        var employeeRepository = new Mock<IEmployeeRepository>();

        employeeRepository
            .Setup(x => x.GetPagedAsync(
                1,
                100,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Employee>());

        employeeRepository
            .Setup(x => x.CountAsync(
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(250);

        var handler = new GetEmployeesHandler(
            employeeRepository.Object);

        var query = new GetEmployeesQuery(
            PageNumber: 1,
            PageSize: 500,
            Search: null);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(100, result.PageSize);
        Assert.Equal(250, result.TotalCount);
        Assert.Equal(3, result.TotalPages);

        employeeRepository.Verify(
            x => x.GetPagedAsync(
                1,
                100,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}