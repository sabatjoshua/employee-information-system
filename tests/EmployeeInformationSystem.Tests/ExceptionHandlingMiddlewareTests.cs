using EmployeeInformationSystem.API.Middleware;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace EmployeeInformationSystem.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldReturn500ProblemDetails_WhenUnhandledExceptionOccurs()
    {
        // Arrange
        RequestDelegate next = _ =>
            throw new InvalidOperationException("Test exception");

        var logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();

        var middleware = new ExceptionHandlingMiddleware(
            next,
            logger.Object);

        var context = new DefaultHttpContext();

        context.Request.Method = HttpMethods.Get;
        context.Request.Path = "/api/test";

        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(
            (int)HttpStatusCode.InternalServerError,
            context.Response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(
            context.Response.Body);

        var responseBody = await reader.ReadToEndAsync();

        Assert.Contains(
            "\"status\":500",
            responseBody);

        Assert.Contains(
            "\"title\":\"An unexpected error occurred.\"",
            responseBody);

        Assert.Contains(
            "\"detail\":\"An internal server error occurred. Please try again later.\"",
            responseBody);

        Assert.Contains(
            "\"instance\":\"/api/test\"",
            responseBody);

        // Important security check:
        // Internal exception details must not be exposed.
        Assert.DoesNotContain(
            "Test exception",
            responseBody);

        logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>(
                    (state, _) =>
                        state.ToString()!.Contains(
                            "Unhandled exception for GET /api/test.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400ProblemDetails_WhenValidationExceptionOccurs()
    {
        // Arrange
        var validationFailures = new[]
        {
            new ValidationFailure(
                "EmployeeNo",
                "Employee number is required."),

            new ValidationFailure(
                "FirstName",
                "First name is required.")
        };

        RequestDelegate next = _ =>
            throw new ValidationException(validationFailures);

        var logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();

        var middleware = new ExceptionHandlingMiddleware(
            next,
            logger.Object);

        var context = new DefaultHttpContext();

        context.Request.Method = HttpMethods.Post;
        context.Request.Path = "/api/Employees";

        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(
            (int)HttpStatusCode.BadRequest,
            context.Response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(
            context.Response.Body);

        var responseBody = await reader.ReadToEndAsync();

        Assert.Contains(
            "\"status\":400",
            responseBody);

        Assert.Contains(
            "\"title\":\"One or more validation errors occurred.\"",
            responseBody);

        Assert.Contains(
            "\"instance\":\"/api/Employees\"",
            responseBody);

        Assert.Contains(
            "Employee number is required.",
            responseBody);

        Assert.Contains(
            "First name is required.",
            responseBody);
    }
}