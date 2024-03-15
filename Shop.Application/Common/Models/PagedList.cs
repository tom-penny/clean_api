namespace Shop.Application.Common.Models;

public class PagedList<T>
{
    public List<T> Items { get; init; }
    public int Page { get; init; }
    public int Size { get; init; }
    public int Count { get; init; }

    public PagedList(List<T> items, int page, int size, int count)
    {
        Items = items;
        Page = page;
        Size = size;
        Count = count;
    }
}