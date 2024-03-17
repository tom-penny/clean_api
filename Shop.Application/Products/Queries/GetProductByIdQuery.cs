namespace Shop.Application.Products.Queries;

using Common.Interfaces;
using Domain.Entities;

public class GetProductByIdQuery : ICachedQuery<Result<Product>>
{
    public required Guid Id { get; init; }
    public string Key => $"Product-{Id}";
    public TimeSpan Expiry => TimeSpan.FromHours(1);
}