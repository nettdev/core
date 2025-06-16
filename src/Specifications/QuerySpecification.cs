using System.Linq.Expressions;

namespace Nett.Core.Specifications;

public abstract class QuerySpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Predicate { get; set; }
    public Expression<Func<TEntity, object>>? OrderBy { get; set; }
    public Expression<Func<TEntity, object>>? OrderByDesceding { get; set; }
    public int? Take { get; set; }
    public int? Skip { get; set; }
}

public abstract class QuerySpecification<T, TResult> : QuerySpecification<T>
{
    public Expression<Func<T, TResult>>? Selector { get; set; }
}
