using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.EmployeeRoles
{
    public sealed class CreateEmployeeRoleValidator
        : AbstractValidator<CreateEmployeeRoleCommand>
    {
        public CreateEmployeeRoleValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.RoleId)
                .NotEqual(Guid.Empty);
        }
    }
}