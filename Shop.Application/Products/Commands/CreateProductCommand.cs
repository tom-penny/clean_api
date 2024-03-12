namespace Shop.Application.Products.Commands;

using Domain.Entities;

public class CreateProductCommand : IRequest<Result<Product>>
{
    public required string Name { get; init; }
    public required int Stock { get; init; }
    public required decimal Price { get; init; }
    public required List<Guid> CategoryIds { get; init; } = new();
}