using EmployeeInformationSystem.Application.Features.FunctionKeys;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreateFunctionKeyValidatorTests
    {
        private readonly CreateFunctionKeyValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new CreateFunctionKeyCommand(
                string.Empty,
                string.Empty,
                new string('A', 501));

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.FunctionCode);

            result.ShouldHaveValidationErrorFor(
                x => x.DisplayName);

            result.ShouldHaveValidationErrorFor(
                x => x.Remarks);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateFunctionKeyCommand(
                "EMPLOYEE_VIEW",
                "View Employees",
                "View employee information");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}