using FluentValidation;

namespace EmployeeInformationSystem.Application.Features.FunctionKeys
{
    public sealed class UpdateFunctionKeyValidator
        : AbstractValidator<UpdateFunctionKeyCommand>
    {
        public UpdateFunctionKeyValidator()
        {
            RuleFor(x => x.FunctionKeyId)
                .NotEqual(Guid.Empty);

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