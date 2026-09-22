using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.RoleFunctions
{
    public sealed class UpdateRoleFunctionValidator
        : AbstractValidator<UpdateRoleFunctionCommand>
    {
        public UpdateRoleFunctionValidator()
        {
            RuleFor(x => x.RoleFunctionId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.RoleId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.FunctionKeyId)
                .NotEqual(Guid.Empty);
        }
    }
}