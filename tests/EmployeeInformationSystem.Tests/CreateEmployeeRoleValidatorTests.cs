using EmployeeInformationSystem.Application.Features.EmployeeRoles;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreateEmployeeRoleValidatorTests
    {
        private readonly CreateEmployeeRoleValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new CreateEmployeeRoleCommand(
                Guid.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.RoleId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateEmployeeRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}