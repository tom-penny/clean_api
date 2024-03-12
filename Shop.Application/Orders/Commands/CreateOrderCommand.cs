namespace Shop.Application.Orders.Commands;

using Domain.Entities;

public record CreateOrderItem(Guid ProductId, int Quantity, decimal UnitPrice);

public class CreateOrderCommand : IRequest<Result<Order>>
{
    public required string CheckoutId { get; init; }
    public required Guid UserId { get; init; }
    public required decimal Amount { get; init; }
    public required List<CreateOrderItem> Items { get; init; }
}