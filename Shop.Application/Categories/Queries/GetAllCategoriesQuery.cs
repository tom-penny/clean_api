namespace Shop.Application.Categories.Queries;

using Common.Interfaces;
using Domain.Entities;

public class GetAllCategoriesQuery : ICachedQuery<Result<List<Category>>>
{
    public string Key => "AllCategories";
    public TimeSpan Expiry => TimeSpan.FromHours(6);
}