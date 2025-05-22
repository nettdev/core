namespace Nett.Core.Models;

public class PaginatedResponse<T>(IEnumerable<T> items, int count, int pageNumber, int limit)
{
    public int Current { get; } = pageNumber;
    public int Pages { get; } = (int)Math.Ceiling(count / (double)limit);
    public int Limit { get; } = limit;
    public int Total { get; } = count;
    public bool HasPrev => Current > 1;
    public bool HasNext => Current < Pages;
    public int PageFirstItem => (Current - 1) * Limit + 1;
    public int PageLastItem => (Current - 1) * Limit + Items.Count();
    public IEnumerable<T> Items { get; } = items;
}
