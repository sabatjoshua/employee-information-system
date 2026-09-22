using EmployeeInformationSystem.Application.Features.RoleFunctions;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdateRoleFunctionValidatorTests
    {
        private readonly UpdateRoleFunctionValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdateRoleFunctionCommand(
                Guid.Empty,
                Guid.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.RoleFunctionId);

            result.ShouldHaveValidationErrorFor(
                x => x.RoleId);

            result.ShouldHaveValidationErrorFor(
                x => x.FunctionKeyId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new UpdateRoleFunctionCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}