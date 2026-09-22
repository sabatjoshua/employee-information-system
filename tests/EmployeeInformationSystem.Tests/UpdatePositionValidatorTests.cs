using EmployeeInformationSystem.Application.Features.Positions;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdatePositionValidatorTests
    {
        private readonly UpdatePositionValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdatePositionCommand(
                Guid.Empty,
                string.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.PositionId);

            result.ShouldHaveValidationErrorFor(
                x => x.Name);

            result.ShouldHaveValidationErrorFor(
                x => x.DepartmentId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new UpdatePositionCommand(
                Guid.NewGuid(),
                "Senior Software Engineer",
                Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}