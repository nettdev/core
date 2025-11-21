using System.Linq.Expressions;

namespace Nett.Core.Models;

[ExcludeFromCodeCoverage]
public abstract class PagedRequest<T, R>
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? OrderBy { get; set; }
    public bool? OrderByDescending { get; set; }
    public string? ThenBy { get; set; }
    public bool? ThenByDescending { get; set; }

    public virtual IReadOnlyCollection<Expression<Func<T, bool>>> ToFilters()
    {
        return [];
    }

    public abstract Expression<Func<T, R>> ToProjection();
}
