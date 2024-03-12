using Microsoft.AspNetCore.Mvc;
using FluentResults;

namespace Shop.API.Extensions;

using Domain.Enums;
using Domain.Primitives;

public static class ResultExtensions
{
    public static IActionResult ToProblem(this Result result)
    {
        if (result.IsSuccess) throw new InvalidOperationException();

        return GenerateProblemDetails(result.Errors);
    }
    
    public static IActionResult ToProblem<T>(this Result<T> result)
    {
        if (result.IsSuccess) throw new InvalidOperationException();

        return GenerateProblemDetails(result.Errors);
    }
    
    private static IActionResult GenerateProblemDetails(List<IError> errors)
    {
        var details = new ProblemDetails
        {
            Title = "Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Extensions = { ["errors"] = errors.Select(e => e.Message) }
        };

        if (errors.Count > 1 || errors.First() is not DomainError error)
        {
            return new ObjectResult(details) { StatusCode = details.Status };
        }
        
        switch (error.Type)
        {
            case ErrorType.Invalid:
                details.Title = "Bad Request";
                details.Status = StatusCodes.Status400BadRequest;
                break;
            case ErrorType.Conflict:
                details.Title = "Conflict";
                details.Status = StatusCodes.Status409Conflict;
                break;
            case ErrorType.NotFound:
                details.Title = "Not Found";
                details.Status = StatusCodes.Status404NotFound;
                break;
            case ErrorType.Unauthorized:
                details.Title = "Unauthorized";
                details.Status = StatusCodes.Status401Unauthorized;
                break;
            case ErrorType.Forbidden:
                details.Title = "Forbidden";
                details.Status = StatusCodes.Status403Forbidden;
                break;
            case ErrorType.Failed:
                details.Title = "Internal Server Error";
                details.Status = StatusCodes.Status500InternalServerError;
                break;
        }
        
        return new ObjectResult(details) { StatusCode = details.Status };
    }
}