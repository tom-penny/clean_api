namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class CategoryMapping
{
    public static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id.Value.ToString(),
            Name = category.Name
        };
    }

    public static CategoriesResponse ToResponse<T>(this IEnumerable<T> list) where T : Category
    {
        return new CategoriesResponse
        {
            Categories = list.Select(c => c.ToResponse()).ToList()
        };
    }
}