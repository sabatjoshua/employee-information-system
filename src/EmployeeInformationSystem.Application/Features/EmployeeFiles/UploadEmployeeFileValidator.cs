using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.EmployeeFiles
{
    public sealed class UploadEmployeeFileValidator
        : AbstractValidator<UploadEmployeeFileCommand>
    {
        public UploadEmployeeFileValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.FileStream)
                .NotNull();

            RuleFor(x => x.OriginalFileName)
                .NotEmpty();

            RuleFor(x => x.ContentType)
                .NotEmpty();

            RuleFor(x => x.FileSize)
                .GreaterThan(0);
        }
    }
}