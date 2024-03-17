namespace Shop.API.Models.Requests;

public class GetAllOrdersRequest
{
    public required string? SortBy { get; init; }
    public required string? OrderBy { get; init; }
    public required int Page { get; init; } = 1;
    public required int Size { get; init; } = 10;
}