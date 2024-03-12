using FluentResults;

namespace Shop.Domain.Primitives;

using Enums;

public class DomainError : Error
{
    public ErrorType Type { get; }

    private DomainError(string message, ErrorType type) : base(message)
    {
        Type = type;
    }
    
    public static DomainError Invalid(string message) =>
        new(message, ErrorType.Invalid);

    public static DomainError Conflict(string message) =>
        new(message, ErrorType.Conflict);
    
    public static DomainError NotFound(string message) =>
        new(message, ErrorType.NotFound);
    
    public static DomainError Unauthorized(string message) =>
        new(message, ErrorType.Unauthorized);
    
    public static DomainError Forbidden(string message) =>
        new(message, ErrorType.Forbidden);

    public static DomainError Failed(string message) =>
        new(message, ErrorType.Failed);
}