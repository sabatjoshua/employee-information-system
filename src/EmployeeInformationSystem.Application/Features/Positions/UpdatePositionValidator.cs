using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Positions
{
    public sealed class UpdatePositionValidator
        : AbstractValidator<UpdatePositionCommand>
    {
        public UpdatePositionValidator()
        {
            RuleFor(x => x.PositionId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.DepartmentId)
                .NotEqual(Guid.Empty);
        }
    }
}