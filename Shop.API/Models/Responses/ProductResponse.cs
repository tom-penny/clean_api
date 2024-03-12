namespace Shop.API.Models.Responses;

public class ProductResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int Stock { get; init; }
    public required decimal Price { get; init; }
    public List<CategoryResponse> Categories { get; init; } = new();
}