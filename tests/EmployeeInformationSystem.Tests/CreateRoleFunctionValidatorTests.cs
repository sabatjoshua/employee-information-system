using EmployeeInformationSystem.Application.Features.RoleFunctions;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreateRoleFunctionValidatorTests
    {
        private readonly CreateRoleFunctionValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new CreateRoleFunctionCommand(
                Guid.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.RoleId);

            result.ShouldHaveValidationErrorFor(
                x => x.FunctionKeyId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateRoleFunctionCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}