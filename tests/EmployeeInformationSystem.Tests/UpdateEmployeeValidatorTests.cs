using EmployeeInformationSystem.Application.Features.Employees;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdateEmployeeValidatorTests
    {
        private readonly UpdateEmployeeValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdateEmployeeCommand(
                Guid.Empty,
                string.Empty,
                string.Empty,
                null,
                string.Empty,
                string.Empty,
                DateTimeOffset.UtcNow,
                "invalid-email",
                null,
                DateTimeOffset.UtcNow,
                Guid.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeNo);

            result.ShouldHaveValidationErrorFor(
                x => x.FirstName);

            result.ShouldHaveValidationErrorFor(
                x => x.LastName);

            result.ShouldHaveValidationErrorFor(
                x => x.GenderCode);

            result.ShouldHaveValidationErrorFor(
                x => x.Email);

            result.ShouldHaveValidationErrorFor(
                x => x.DepartmentId);

            result.ShouldHaveValidationErrorFor(
                x => x.PositionId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new UpdateEmployeeCommand(
                Guid.NewGuid(),
                "100001",
                "John",
                "Michael",
                "Smith",
                "M",
                new DateTimeOffset(1990, 1, 1, 0, 0, 0, TimeSpan.Zero),
                "john.smith@example.com",
                "91234567",
                new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}