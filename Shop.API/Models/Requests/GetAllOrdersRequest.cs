namespace Shop.API.Models.Requests;

public class GetAllOrdersRequest
{
    public required string? Sort { get; init; }
    public required string? Order { get; init; }
    public required int Page { get; init; } = 1;
    public required int Size { get; init; } = 10;
}