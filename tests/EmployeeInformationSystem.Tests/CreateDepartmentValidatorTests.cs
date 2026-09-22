using EmployeeInformationSystem.Application.Features.Departments;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreateDepartmentValidatorTests
    {
        private readonly CreateDepartmentValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenNameIsInvalid()
        {
            var command = new CreateDepartmentCommand(
                string.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.Name);
        }

        [Fact]
        public void Validate_ShouldPass_WhenNameIsValid()
        {
            var command = new CreateDepartmentCommand(
                "Human Resources");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}