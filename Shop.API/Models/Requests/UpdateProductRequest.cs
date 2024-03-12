namespace Shop.API.Models.Requests;

public class UpdateProductRequest
{
    public required string Name { get; init; }
    public required int Stock { get; init; }
    public required decimal Price { get; init; }
    public required List<Guid> CategoryIds { get; init; } = new();
}