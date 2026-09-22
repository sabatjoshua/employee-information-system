using EmployeeInformationSystem.Application.Features.EmployeeFiles;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class GetEmployeeFilesQueryValidatorTests
    {
        private readonly GetEmployeeFilesQueryValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenEmployeeIdIsInvalid()
        {
            var query = new GetEmployeeFilesQuery(
                Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenEmployeeIdIsValid()
        {
            var query = new GetEmployeeFilesQuery(
                Guid.NewGuid());

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}