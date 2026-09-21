using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationSystem.API.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                await HandleValidationExceptionAsync(
                    context,
                    exception);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Unhandled exception for {Method} {Path}.",
                    context.Request.Method,
                    context.Request.Path);

                await HandleExceptionAsync(context);
            }
        }

        private static async Task HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException exception)
        {
            const int statusCode = StatusCodes.Status400BadRequest;

            var errors = exception.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .ToArray());

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = statusCode,
                Title = "One or more validation errors occurred.",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);

            await context.Response.WriteAsync(json);
        }

        private static async Task HandleExceptionAsync(
            HttpContext context)
        {
            const int statusCode = StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "An unexpected error occurred.",
                Detail = "An internal server error occurred. Please try again later.",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);

            await context.Response.WriteAsync(json);
        }
    }
}