namespace Shop.Application.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Result<Unit>>;