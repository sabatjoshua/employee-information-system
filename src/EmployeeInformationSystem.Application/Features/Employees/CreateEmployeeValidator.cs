using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Employees
{
    public sealed class CreateEmployeeValidator
        : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.EmployeeNo)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.GenderCode)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.DepartmentId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.PositionId)
                .NotEqual(Guid.Empty);
        }
    }
}