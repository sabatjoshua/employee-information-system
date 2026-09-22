using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Departments
{
    public sealed class UpdateDepartmentValidator
        : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.DepartmentId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}