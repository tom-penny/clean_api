namespace Shop.Domain.Errors;

using Primitives;

public static class UserError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"User {id} not found");

    public static DomainError EmailExists() =>
        DomainError.Conflict("Email already in use");

    public static DomainError RegistrationFailed() =>
        DomainError.Failed("Registration failed");

    public static DomainError InvalidCredentials() =>
        DomainError.Unauthorized("Invalid credentials");

    public static DomainError InvalidToken() =>
        DomainError.Invalid("Invalid token");
}