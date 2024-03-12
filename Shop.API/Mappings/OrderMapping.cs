namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse
        {
            Id = order.Id.Value.ToString(),
            CheckoutId = order.CheckoutId.Value,
            UserId = order.UserId.ToString(),
            Status = order.Status.ToString(),
            Amount = order.Amount,
            Items = order.Items.Select(i => i.ToResponse()).ToList(),
            PaymentId = order.PaymentId?.Value.ToString()
        };
    }
}