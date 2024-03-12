namespace Shop.Domain.Entities;

using Primitives;

public record OrderItemId(Guid Value);

public class OrderItem : BaseEntity
{
    public OrderItemId Id { get; init; }
    public OrderId OrderId { get; init; }
    public ProductId ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    
    public OrderItem(OrderItemId id, OrderId orderId, ProductId productId, decimal unitPrice, int quantity)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
    
    private OrderItem() {}
}