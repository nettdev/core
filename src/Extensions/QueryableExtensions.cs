using System.Linq.Expressions;
using Nett.Core.Domain;
using Nett.Core.Models;

namespace Nett.Core.Extensions;

public static class QueryableExtensions
{
    public const int PaginationLimit = 10;
    public const int InitialPage = 1;

    public static IQueryable<T> Apply<T>(this IQueryable<T> queryable, Dictionary<string, Expression<Func<T, object>>> sortMap, PaginatedRequest request) where T : Entity
    {
        var query = ApplyOrderBy(queryable, sortMap, request);
        var orderedQuery = ApplyThenBy(query, sortMap, request);
        var paginatedQuery = ApplyPagination(orderedQuery, request);

        return paginatedQuery;
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> query, bool apply, Expression<Func<T, bool>> predicate)
    {
        return apply ? query.Where(predicate) : query;
    }

    private static IOrderedQueryable<T> ApplyOrderBy<T>(IQueryable<T> queryable, Dictionary<string, Expression<Func<T, object>>> sortMap, PaginatedRequest request) where T : Entity
    {
        if (request.OrderBy is { } orderByDesc && request.OrderByDescending)
            return queryable.OrderByDescending(GetExpression(sortMap, orderByDesc));

        if (request.OrderBy is { } orderBy)
            return queryable.OrderBy(GetExpression(sortMap, orderBy));

        return queryable.OrderByDescending(x => x.Id);
    }

    private static IOrderedQueryable<T> ApplyThenBy<T>(IOrderedQueryable<T> queryable, Dictionary<string, Expression<Func<T, object>>> sortMap, PaginatedRequest request)
    {
        if (request.ThenBy is { } thenBy && !request.ThenByDescending)
            return queryable.ThenBy(GetExpression(sortMap, thenBy));

        if (request.ThenBy is { } thenByDesc && request.ThenByDescending)
            return queryable.ThenByDescending(GetExpression(sortMap, thenByDesc));

        return queryable;
    }

    private static IQueryable<T> ApplyPagination<T>(IOrderedQueryable<T> queryable, PaginatedRequest request)
    {
        var page = request.Page ?? InitialPage;
        var limit = request.Limit ?? PaginationLimit;
        return queryable.Skip((page - 1) * limit).Take(limit);
    }

    private static Expression<Func<T, object>> GetExpression<T>(Dictionary<string, Expression<Func<T, object>>> sortMap, string key) =>
        sortMap.TryGetValue(key, out var expression) ? expression : throw new ArgumentException($"Invalid sort field: {key}");
}
