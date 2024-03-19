namespace Shop.Application.Products.Queries;

using Common.Models;
using Common.Interfaces;
using Domain.Entities;

public class GetProductsByCategoryQuery : ICachedQuery<Result<PagedList<Product>>>
{
    public required string IdOrSlug { get; init; }
    public required string? SortBy { get; init; }
    public required string? OrderBy { get; init; }
    public required int Page { get; init; }
    public required int Size { get; init; }
    public string Key => $"ProductsByCategory-{IdOrSlug}-Sort-{SortBy}-Order-{OrderBy}-Page-{Page}-Size-{Size}";
    public TimeSpan Expiry => TimeSpan.FromHours(1);
}