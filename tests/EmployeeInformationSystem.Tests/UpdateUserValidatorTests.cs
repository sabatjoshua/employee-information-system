using EmployeeInformationSystem.Application.Features.Users;
using FluentValidation.TestHelper;

namespace EmployeeInformationSystem.Tests
{
    public class UpdateUserValidatorTests
    {
        private readonly UpdateUserValidator _validator = new();

        [Fact]
        public void Validate_ShouldFail_WhenRequiredFieldsAreInvalid()
        {
            var command = new UpdateUserCommand(
                Guid.Empty,
                Guid.Empty,
                string.Empty,
                "123",
                false,
                false);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(
                x => x.UserId);

            result.ShouldHaveValidationErrorFor(
                x => x.EmployeeId);

            result.ShouldHaveValidationErrorFor(
                x => x.UserName);

            result.ShouldHaveValidationErrorFor(
                x => x.Password);
        }

        [Fact]
        public void Validate_ShouldPass_WhenPasswordIsNotProvided()
        {
            var command = new UpdateUserCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "john.doe",
                null,
                false,
                false);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}