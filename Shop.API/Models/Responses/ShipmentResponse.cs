namespace Shop.API.Models.Responses;

public class ShipmentResponse
{
    public required string Id { get; init; }
    public required string OrderId { get; init; }
    public DateTime? DateDispatched { get; init; }
    public DateTime? DateDelivered { get; init; }
}