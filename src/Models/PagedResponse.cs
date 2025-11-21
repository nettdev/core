namespace Nett.Core.Models;

public sealed class PagedResponse<T>(IEnumerable<T> items, int count, int page, int limit)
{
    public int Page => page;
    public int Limit => limit;
    public int TotalPages => (int)Math.Ceiling(count / (double)limit);
    public int TotalCount => count;
    public bool HasPrev => Page > 1;
    public bool HasNext => Page < TotalPages;
    public IEnumerable<T> Items => items;
}
