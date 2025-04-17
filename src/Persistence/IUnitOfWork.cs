namespace Nett.Core.Persistence;

public interface IUnitOfWork
{
    Task<bool> Commit(CancellationToken cancellationToken = default);
}