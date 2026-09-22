using EmployeeInformationSystem.Application.Features.Departments;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdateDepartmentValidatorTests
    {
        private readonly UpdateDepartmentValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdateDepartmentCommand(
                Guid.Empty,
                string.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.DepartmentId);

            result.ShouldHaveValidationErrorFor(
                x => x.Name);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new UpdateDepartmentCommand(
                Guid.NewGuid(),
                "Human Resources");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}