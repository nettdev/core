namespace Nett.Core;

public class PagedRequest(int page, int limit)
{
    public int Page { get; } = page;
    public int Limit { get; } = limit;
    public int Offset { get; } = (page - 1) * limit;
}
