using Microsoft.AspNetCore.Mvc;

namespace Shop.API.Middleware;

using FluentValidation;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationExceptionMiddleware> _logger;

    public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
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
            _logger.LogError(exception, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var details = new ValidationProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
            };

            foreach (var error in exception.Errors)
            {
                details.Errors.Add(error.PropertyName, new []{ error.ErrorMessage });
            }

            await context.Response.WriteAsJsonAsync(details);
        }
    }
}