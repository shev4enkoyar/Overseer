namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<Result<int>> TrySaveChangesAsync(CancellationToken cancellationToken);
}
