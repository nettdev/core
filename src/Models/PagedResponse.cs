namespace Nett.Core.Models;

public sealed class PagedResponse<T>(IEnumerable<T> items, int count, int page, int limit)
{
    public int Page => page;
    public int Limit => limit;
    public int Pages => (int)Math.Ceiling(count / (double)limit);
    public int Count => count;
    public bool HasPrev => Page > 1;
    public bool HasNext => Page < Pages;
    public IEnumerable<T> Items => items;
}
