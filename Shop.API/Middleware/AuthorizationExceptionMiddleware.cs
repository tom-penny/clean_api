using Microsoft.AspNetCore.Mvc;

namespace Shop.API.Middleware;

public class AuthorizationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationExceptionMiddleware> _logger;

    public AuthorizationExceptionMiddleware(RequestDelegate next, ILogger<AuthorizationExceptionMiddleware> logger)
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
        catch (UnauthorizedAccessException exception)
        {
            _logger.LogError(exception, exception.Message);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var details = new ProblemDetails
            {
                Title = "Forbidden",
                Status = StatusCodes.Status403Forbidden,
            };

            await context.Response.WriteAsJsonAsync(details);
        }
    }
}