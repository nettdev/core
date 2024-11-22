namespace Nett.Core;

public class PagedResponse<T>(IEnumerable<T> items, int count, int pageNumber, int limit)
{
    public int Current { get; } = pageNumber;
    public int Pages { get; } = (int)Math.Ceiling(count / (double)limit);
    public int Limit { get; } = limit;
    public int Total { get; } = count;
    public bool HasPrev => Current > 1;
    public bool HasNext => Current < Pages;
    public IEnumerable<T> Items { get; } = items;
}
