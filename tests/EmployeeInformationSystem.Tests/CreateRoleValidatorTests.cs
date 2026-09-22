using EmployeeInformationSystem.Application.Features.Roles;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreateRoleValidatorTests
    {
        private readonly CreateRoleValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new CreateRoleCommand(
                string.Empty,
                new string('A', 501));

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.Name);

            result.ShouldHaveValidationErrorFor(
                x => x.Description);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateRoleCommand(
                "HR Administrator",
                "Manages employees and user accounts.");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}