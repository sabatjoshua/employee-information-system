using EmployeeInformationSystem.Application.Common.Behaviors;
using FluentValidation;
using MediatR;

namespace EmployeeInformationSystem.Tests;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_ShouldCallNext_WhenRequestIsValid()
    {
        // Arrange
        var request = new TestRequest("valid");

        var validator = new TestRequestValidator();

        var behavior = new ValidationBehavior<TestRequest, string>(
            new[] { validator });

        var nextCalled = false;

        Task<string> Next(
            CancellationToken cancellationToken)
        {
            nextCalled = true;
            return Task.FromResult("success");
        }

        // Act
        var result = await behavior.Handle(
            request,
            Next,
            CancellationToken.None);

        // Assert
        Assert.Equal("success", result);
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new TestRequest("");

        var validator = new TestRequestValidator();

        var behavior = new ValidationBehavior<TestRequest, string>(
            new[] { validator });

        var nextCalled = false;

        Task<string> Next(
            CancellationToken cancellationToken)
        {
            nextCalled = true;
            return Task.FromResult("success");
        }

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(
                request,
                Next,
                CancellationToken.None));

        Assert.False(nextCalled);
    }

    private sealed record TestRequest(string Value)
        : IRequest<string>;

    private sealed class TestRequestValidator
        : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty();
        }
    }
}