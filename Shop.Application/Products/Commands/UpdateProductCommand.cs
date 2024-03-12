namespace Shop.Application.Products.Commands;

public class UpdateProductCommand : IRequest<Result<Unit>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int Stock { get; init; }
    public required decimal Price { get; init; }
    public required List<Guid> CategoryIds { get; init; } = new();
}