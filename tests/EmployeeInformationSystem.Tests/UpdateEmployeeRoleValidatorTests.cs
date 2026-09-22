using EmployeeInformationSystem.Application.Features.EmployeeRoles;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdateEmployeeRoleValidatorTests
    {
        private readonly UpdateEmployeeRoleValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdateEmployeeRoleCommand(
                Guid.Empty,
                Guid.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeRoleId);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.RoleId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new UpdateEmployeeRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}