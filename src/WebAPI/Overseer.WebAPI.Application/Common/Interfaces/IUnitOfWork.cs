namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<Fin<int>> TrySaveChangesAsync(CancellationToken cancellationToken);
}
