namespace Shop.Domain.Entities;

using Enums;
using Primitives;

public record PaymentId(Guid Value);

public class Payment : BaseEntity
{
    public PaymentId Id { get; init; }
    public CheckoutId CheckoutId { get; init; }
    public decimal Amount { get; init; }
    public PaymentStatus Status { get; set; }
    
    public Payment(PaymentId id, CheckoutId checkoutId, decimal amount, PaymentStatus status)
    {
        Id = id;
        CheckoutId = checkoutId;
        Amount = amount;
        Status = status;
    }
    
    private Payment() {}
}