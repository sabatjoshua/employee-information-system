using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.EmployeeFiles
{
    public sealed class DownloadEmployeeFileQueryValidator
        : AbstractValidator<DownloadEmployeeFileQuery>
    {
        public DownloadEmployeeFileQueryValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.FileId)
                .NotEqual(Guid.Empty);
        }
    }
}