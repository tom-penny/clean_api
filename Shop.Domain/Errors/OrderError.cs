namespace Shop.Domain.Errors;

using Primitives;

public static class OrderError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"Order {id} not found");
    
    public static DomainError UserNotFound(Guid id) =>
        DomainError.NotFound($"User {id} not found");
    
    public static DomainError ProductNotFound(Guid id) =>
        DomainError.NotFound($"Product {id} not found");
    
    public static DomainError ProductOutOfStock(Guid id) =>
        DomainError.Conflict($"Product {id} out of stock");
}