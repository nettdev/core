using Nett.Core.Models;

namespace Nett.Core.Domain;

public interface IRepository<T> where T : AggregateRoot
{
    Task<PagedResponse<R>> Query<R>(PagedRequest<T, R> request, CancellationToken cancellation);
}
