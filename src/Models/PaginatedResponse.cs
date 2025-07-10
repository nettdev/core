namespace Nett.Core.Models;

public class PaginatedResponse<T>(IEnumerable<T> items, int count, PaginatedRequest request)
{
    public int Current { get; } = request.Page ?? PaginatedRequest.DefaultPage;
    public int Pages { get; } = (int)Math.Ceiling(count / (double)(request.Limit ?? PaginatedRequest.DefaultLimit));
    public int Limit { get; } = request.Limit ?? PaginatedRequest.DefaultLimit;
    public int Total { get; } = count;
    public bool HasPrev => Current > 1;
    public bool HasNext => Current < Pages;
    public int PageFirstItem => (Current - 1) * Limit + 1;
    public int PageLastItem => (Current - 1) * Limit + Items.Count();
    public IEnumerable<T> Items { get; } = items;
}
