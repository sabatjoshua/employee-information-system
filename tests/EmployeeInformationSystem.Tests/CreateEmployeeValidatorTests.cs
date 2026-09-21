using EmployeeInformationSystem.Application.Features.Employees;

namespace EmployeeInformationSystem.Tests;

public class CreateEmployeeValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
    {
        // Arrange
        var validator = new CreateEmployeeValidator();

        var command = new CreateEmployeeCommand(
            "",
            "",
            null,
            "",
            "",
            DateTimeOffset.UtcNow,
            "invalid-email",
            null,
            DateTimeOffset.UtcNow,
            Guid.Empty,
            Guid.Empty);

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "EmployeeNo");

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "FirstName");

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "LastName");

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "GenderCode");

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "Email");

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "DepartmentId");

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == "PositionId");
    }

    [Fact]
    public async Task Validate_ShouldPass_WhenRequestIsValid()
    {
        // Arrange
        var validator = new CreateEmployeeValidator();

        var command = new CreateEmployeeCommand(
            "EMP-001",
            "John",
            null,
            "Smith",
            "M",
            new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero),
            "john.smith@example.com",
            null,
            new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }
}