using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.FunctionKeys
{
    public sealed class CreateFunctionKeyValidator
        : AbstractValidator<CreateFunctionKeyCommand>
    {
        public CreateFunctionKeyValidator()
        {
            RuleFor(x => x.FunctionCode)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Remarks)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
        }
    }
}