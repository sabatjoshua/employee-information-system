using EmployeeInformationSystem.Application.Features.Roles;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdateRoleValidatorTests
    {
        private readonly UpdateRoleValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdateRoleCommand(
                Guid.Empty,
                string.Empty,
                new string('A', 501));

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.RoleId);

            result.ShouldHaveValidationErrorFor(
                x => x.Name);

            result.ShouldHaveValidationErrorFor(
                x => x.Description);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new UpdateRoleCommand(
                Guid.NewGuid(),
                "HR Administrator",
                null);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}