namespace Shop.API.Models.Requests;

public class CreateShipmentRequest
{
    public required Guid OrderId { get; init; }
}