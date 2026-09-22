using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Users
{
    public sealed class CreateUserValidator
        : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}