using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed class CreateDepartmentValidator
        : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}