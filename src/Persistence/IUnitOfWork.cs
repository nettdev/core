namespace Nett.Core;

public interface IUnitOfWork
{
    Task<Result<Guid, Error>> Commit(Guid id);
    Task<Result<bool, Error>> Commit();
}