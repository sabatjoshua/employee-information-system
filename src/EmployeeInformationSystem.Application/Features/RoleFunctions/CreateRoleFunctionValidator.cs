using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.RoleFunctions
{
    public sealed class CreateRoleFunctionValidator
        : AbstractValidator<CreateRoleFunctionCommand>
    {
        public CreateRoleFunctionValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.FunctionKeyId)
                .NotEqual(Guid.Empty);
        }
    }
}