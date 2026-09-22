using EmployeeInformationSystem.Application.Features.EmployeeFiles;
using FluentValidation.TestHelper;
using System.Text;

namespace EmployeeInformationSystem.Tests
{
    public class UploadEmployeeFileValidatorTests
    {
        private readonly UploadEmployeeFileValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UploadEmployeeFileCommand(
                Guid.Empty,
                null!,
                string.Empty,
                string.Empty,
                0);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.FileStream);

            result.ShouldHaveValidationErrorFor(
                x => x.OriginalFileName);

            result.ShouldHaveValidationErrorFor(
                x => x.ContentType);

            result.ShouldHaveValidationErrorFor(
                x => x.FileSize);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes("test file"));

            var command = new UploadEmployeeFileCommand(
                Guid.NewGuid(),
                stream,
                "document.pdf",
                "application/pdf",
                stream.Length);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}