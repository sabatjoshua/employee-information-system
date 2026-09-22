using EmployeeInformationSystem.Application.Features.Positions;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreatePositionValidatorTests
    {
        private readonly CreatePositionValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new CreatePositionCommand(
                string.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.Name);

            result.ShouldHaveValidationErrorFor(
                x => x.DepartmentId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreatePositionCommand(
                "Software Engineer",
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}