using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.EmployeeRoles
{
    public sealed class UpdateEmployeeRoleValidator
        : AbstractValidator<UpdateEmployeeRoleCommand>
    {
        public UpdateEmployeeRoleValidator()
        {
            RuleFor(x => x.EmployeeRoleId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.RoleId)
                .NotEqual(Guid.Empty);
        }
    }
}