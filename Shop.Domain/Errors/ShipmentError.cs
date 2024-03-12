namespace Shop.Domain.Errors;

using Primitives;

public static class ShipmentError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"Shipment {id} not found");
    
    public static DomainError OrderNotFound(Guid id) =>
        DomainError.NotFound($"Order {id} not found");
}