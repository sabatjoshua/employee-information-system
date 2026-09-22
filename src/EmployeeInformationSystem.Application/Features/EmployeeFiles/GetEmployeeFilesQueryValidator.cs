using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.EmployeeFiles
{
    public sealed class GetEmployeeFilesQueryValidator
        : AbstractValidator<GetEmployeeFilesQuery>
    {
        public GetEmployeeFilesQueryValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);
        }
    }
}