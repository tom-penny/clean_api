namespace Shop.Application.Categories.Queries;

using Common.Interfaces;
using Domain.Entities;

public class GetCategoryByIdQuery : ICachedQuery<Result<Category>>
{
    public required Guid Id { get; init; }
    public string Key => $"Category-{Id}";
    public TimeSpan Expiry => TimeSpan.FromHours(6);
}