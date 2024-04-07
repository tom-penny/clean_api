namespace Shop.Domain.Errors;

using Primitives;

public static class AddressError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"Address {id} not found");

    public static DomainError UserNotFound(Guid id) =>
        DomainError.NotFound($"User {id} not found");
}