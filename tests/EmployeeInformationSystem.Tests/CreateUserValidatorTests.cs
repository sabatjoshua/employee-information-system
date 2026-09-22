using EmployeeInformationSystem.Application.Features.Users;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new CreateUserCommand(
                Guid.Empty,
                string.Empty,
                "123");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.UserName);

            result.ShouldHaveValidationErrorFor(
                x => x.Password);
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            var command = new CreateUserCommand(
                Guid.NewGuid(),
                "john.doe",
                "Password123");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}