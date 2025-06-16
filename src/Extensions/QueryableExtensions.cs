using System.Linq.Expressions;
using Nett.Core.Specifications;

namespace Nett.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> Apply<TEntity>(this IQueryable<TEntity> queryable, QuerySpecification<TEntity> specification) where TEntity : class
    {
        IQueryable<TEntity> query = queryable;

        if (specification.Predicate is {} predicate)
            query = query.Where(predicate);

        if (specification.OrderBy is {} orderBy)
            query = query.OrderBy(orderBy);

        if (specification.OrderByDesceding is {} orderByDesceding)
            query = query.OrderByDescending(orderByDesceding);

        if (specification.Skip is {} skip)
            query = query.Skip(skip);

        if (specification.Take is {} take)
            query = query.Take(take);

        return query;
    }

    public static IQueryable<TResult> CreateQuery<TEntity, TResult>(this IQueryable<TEntity> input, QuerySpecification<TEntity, TResult> specification) where TEntity : class
    {
        var queryable = Apply(input, specification);

        if (specification.Selector is null)
            throw new ArgumentException("Selector cannot be null");

        return queryable.Select(specification.Selector);
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> query, bool apply, Expression<Func<T, bool>> predicate) 
    {
        return apply ? query.Where(predicate) : query; 
    }
    
    public static IOrderedQueryable<T> OrderByColumn<T>(this IQueryable<T> source, string columnPath, string direction = "asc")
    {
        return direction.ToLower() switch
        {
            "desc" => source.OrderByColumnUsing(columnPath, "OrderByDescending"),
            _ => source.OrderByColumnUsing(columnPath, "OrderBy")
        };
    }

    public static IOrderedQueryable<T> ThenByColumn<T>(this IOrderedQueryable<T> source, string columnPath, string direction = "asc") 
    {
        return direction.ToLower() switch
        {
            "desc" => source.OrderByColumnUsing(columnPath, "ThenByDescending"),
            _ => source.OrderByColumnUsing(columnPath, "ThenBy")
        };
    }

    private static IOrderedQueryable<T> OrderByColumnUsing<T>(this IQueryable<T> source, string columnPath, string method)
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        var member = columnPath.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
        var keySelector = Expression.Lambda(member, parameter);
        var methodCall = Expression.Call(typeof(Queryable), method, [parameter.Type, member.Type], source.Expression, Expression.Quote(keySelector));

        return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
    }
}
