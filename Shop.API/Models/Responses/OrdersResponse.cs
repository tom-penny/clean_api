namespace Shop.API.Models.Responses;

public class OrdersResponse : PagedResponse
{
    public required List<OrderResponse> Orders { get; init; }
}