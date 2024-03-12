namespace Shop.Application.Products.Queries;

using Domain.Entities;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<Product>>;