namespace Shop.API.Models.Responses;

public class CategoriesResponse
{
    public required List<CategoryResponse> Categories { get; init; }
}