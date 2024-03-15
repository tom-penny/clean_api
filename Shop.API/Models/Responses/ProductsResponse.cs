namespace Shop.API.Models.Responses;

public class ProductsResponse : PagedResponse
{
    public required List<ProductResponse> Products { get; init; }
}