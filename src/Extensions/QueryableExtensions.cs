using System.Linq.Expressions;

namespace Nett.Core.Extensions;

public static class QueryableExtensions
{
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
