using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nett.Core.Domain;
using Nett.Core.Models;

namespace Nett.Core.Persistence;

public abstract class Repository<T> : IRepository<T> where T : AggregateRoot
{
    protected abstract IQueryable<T> Queryable { get; }
    protected abstract Dictionary<string, Expression<Func<T, object>>> SortMap { get; }

    public async Task<PagedResponse<R>> Query<R>(PagedRequest<T, R> request, CancellationToken cancellation)
    {
        var page = request.Page ?? 1;
        var limit = request.Limit ?? 10;

        IQueryable<T> query = Queryable;
        query = ApplyFilters(query, request);
        var total = await query.CountAsync(cancellation);
        query = ApplySorting(query, request);
        query =  ApplyPagination<T>(query, page, limit);
        var items = await ApplyProjection(query, request).ToListAsync(cancellation);
        return new PagedResponse<R>(items, total, page, limit);
    }

    private static IQueryable<T> ApplyFilters<R>(IQueryable<T> source, PagedRequest<T, R> request)
    {
        foreach (var predicate in request.ToFilters())
        {
            source = source.Where(predicate);
        }

        return source;
    }

    private static IQueryable<R> ApplyProjection<R>(IQueryable<T> query, PagedRequest<T, R> request)
    {
        return query.Select(request.ToProjection());
    }

    private IQueryable<T> ApplySorting<R>(IQueryable<T> source, PagedRequest<T, R> request)
    {
        IOrderedQueryable<T> ordered = ResolvePrimaryOrdering(source, request);
        ordered = ResolveSecondaryOrdering(ordered, request);
        return ordered;
    }

    private IOrderedQueryable<T> ResolvePrimaryOrdering<R>(IQueryable<T> source, PagedRequest<T, R> request)
    {
        if (string.IsNullOrWhiteSpace(request.OrderBy))
            return source.OrderBy(SortMap.First().Value);

        var sortExpression = GetSort(request.OrderBy);

        return request.OrderByDescending is true ? source.OrderByDescending(sortExpression) : source.OrderBy(sortExpression);
    }

    private IOrderedQueryable<T> ResolveSecondaryOrdering<R>(IOrderedQueryable<T> source, PagedRequest<T, R> request)
    {
        if (string.IsNullOrWhiteSpace(request.ThenBy))
            return source;

        var sortExpression = GetSort(request.ThenBy);

        return request.ThenByDescending is true ? source.ThenByDescending(sortExpression) : source.ThenBy(sortExpression);
    }

    private static IQueryable<T> ApplyPagination<R>(IQueryable<T> source, int page, int limit)
    {
        var skip = (page - 1) * limit;
        return source.Skip(skip).Take(limit);
    }

    private Expression<Func<T, object>> GetSort(string field) =>
        SortMap.TryGetValue(field, out var value) ? value : throw new InvalidSortFieldException(field);
}

public sealed class InvalidSortFieldException(string field) : Exception($"Invalid sort field: {field}");
