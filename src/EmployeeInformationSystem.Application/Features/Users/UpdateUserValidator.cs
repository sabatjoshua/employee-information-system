using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.Users
{
    public sealed class UpdateUserValidator
        : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.EmployeeId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }
    }
}