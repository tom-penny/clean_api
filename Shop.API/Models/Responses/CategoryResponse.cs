namespace Shop.API.Models.Responses;

public class CategoryResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Slug { get; init; }
}