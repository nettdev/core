namespace Nett.Core;

public interface IUnitOfWork : IDisposable
{
    Task Commit(CancellationToken cancellationToken = default(CancellationToken));
}