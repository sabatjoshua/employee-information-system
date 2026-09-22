using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed class CreatePositionValidator
        : AbstractValidator<CreatePositionCommand>
    {
        public CreatePositionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.DepartmentId)
                .NotEqual(Guid.Empty);
        }
    }
}