namespace Shop.Domain.Errors;

using Primitives;

public static class ProductError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"Product {id} not found");
}