using EmployeeInformationSystem.Application.Features.EmployeeFiles;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class DownloadEmployeeFileQueryValidatorTests
    {
        private readonly DownloadEmployeeFileQueryValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var query = new DownloadEmployeeFileQuery(
                Guid.Empty,
                Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.FileId);
        }

        [Fact]
        public void Validate_ShouldPass_WhenQueryIsValid()
        {
            var query = new DownloadEmployeeFileQuery(
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}