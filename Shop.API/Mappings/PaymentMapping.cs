namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class PaymentMapping
{
    public static PaymentResponse ToResponse(this Payment payment)
    {
        return new PaymentResponse
        {
            Id = payment.Id.Value.ToString(),
            CheckoutId = payment.CheckoutId.Value,
            Status = payment.Status.ToString(),
            Amount = payment.Amount
        };
    }
}