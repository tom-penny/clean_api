namespace Shop.API.Models.Requests;

public class OrderItemRequest
{
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
};

public class CreateOrderRequest
{
    public required string CheckoutId { get; init; }
    public required Guid UserId { get; init; }
    public required decimal Amount { get; init; }
    public required List<OrderItemRequest> Items { get; init; }
}