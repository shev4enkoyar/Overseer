namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IMinIoManager
{
    Task<Result<string>> CreateNewMinIoContainerAsync(string containerName, string userName, string password,
        CancellationToken cancellationToken);
}
