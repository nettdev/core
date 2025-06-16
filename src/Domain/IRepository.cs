namespace Nett.Core.Domain;

public interface IRepository<T> where T : IAggregateRoot
{
    IQueryable<T> Queryable { get; }
}
