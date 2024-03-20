namespace Shop.API.Mappings;

using Models.Responses;
using Domain.Entities;
using Application.Common.Models;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse
        {
            Id = order.Id.Value.ToString(),
            CheckoutId = order.CheckoutId.Value,
            UserId = order.UserId.Value.ToString(),
            Status = order.Status.ToString(),
            Amount = order.Amount,
            Created = order.Created,
            Items = order.Items.Select(i => i.ToResponse()).ToList(),
            PaymentId = order.PaymentId?.Value.ToString()
        };
    }
    
    public static OrdersResponse ToResponse<T>(this PagedList<T> list) where T : Order
    {
        return new OrdersResponse
        {
            Page = list.Page,
            Size = list.Size,
            Count = list.Count,
            Orders = list.Items.Select(o => o.ToResponse()).ToList(),
        };
    }
}