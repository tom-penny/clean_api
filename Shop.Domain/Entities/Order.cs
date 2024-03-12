namespace Shop.Domain.Entities;

using Enums;
using Events;
using Primitives;

public record OrderId(Guid Value);
public record CheckoutId(string Value);

public class Order : BaseEntity
{
    public OrderId Id { get; init; }
    public CheckoutId CheckoutId { get; init; }
    public UserId UserId { get; init; }
    public decimal Amount { get; init; }
    public OrderStatus Status { get; private set; }
    public List<OrderItem> Items { get; init; } = new();
    public PaymentId? PaymentId { get; private set; }
    
    public Order(OrderId id, CheckoutId checkoutId, UserId userId, decimal amount, OrderStatus status)
    {
        Id = id;
        CheckoutId = checkoutId;
        UserId = userId;
        Amount = amount;
        Status = status;
    }

    public void AddPayment(Payment payment)
    {
        PaymentId = payment.Id;
        
        if (Amount == payment.Amount && payment.Status == PaymentStatus.Approved)
        {
            SetConfirmed();
        }
        else
        {
            SetCancelled();
        }
    }
    
    public void SetConfirmed()
    {
        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmed(this));
    }

    public void SetProcessing()
    {
        Status = OrderStatus.Processing;
        AddDomainEvent(new OrderProcessing(this));
    }

    public void SetCompleted()
    {
        Status = OrderStatus.Completed;
        AddDomainEvent(new OrderCompleted(this));
    }

    public void SetCancelled()
    {
        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelled(this));
    }
    
    private Order() {}
}