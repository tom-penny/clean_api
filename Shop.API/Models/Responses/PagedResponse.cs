namespace Shop.API.Models.Responses;

public abstract class PagedResponse
{
    public required int Page { get; init; }
    public required int Size { get; init; }
    public required int Count { get; init; }
    public bool HasNextPage => Page * Size < Count;
    public bool HasPreviousPage => Page > 1;
}