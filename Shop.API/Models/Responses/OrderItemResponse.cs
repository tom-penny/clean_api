namespace Shop.API.Models.Responses;

public class OrderItemResponse
{
    public required string Id { get; init; }
    public required string OrderId { get; init; }
    public required string ProductId { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
}