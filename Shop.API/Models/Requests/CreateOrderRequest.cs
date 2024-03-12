namespace Shop.API.Models.Requests;

public record OrderItemRequest(Guid ProductId, int Quantity, decimal UnitPrice);

public class CreateOrderRequest
{
    public required string CheckoutId { get; init; }
    public required Guid UserId { get; init; }
    public required decimal Amount { get; init; }
    public required List<OrderItemRequest> Items { get; init; }
}