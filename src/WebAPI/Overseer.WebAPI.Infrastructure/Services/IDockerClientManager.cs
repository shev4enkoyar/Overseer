using Docker.DotNet.Models;

namespace Overseer.WebAPI.Infrastructure.Services;

public interface IDockerClientManager
{
    Task<Result> StartContainerById(string containerId, CancellationToken cancellationToken);

    Task<Result> StartContainerByName(string containerName, CancellationToken cancellationToken);

    Task<Result> StopContainerById(string containerId, CancellationToken cancellationToken);

    Task<Result> StopContainerByName(string containerName, CancellationToken cancellationToken);

    Task<Result> RemoveContainerById(string containerId, CancellationToken cancellationToken);

    Task<Result> RemoveContainerByName(string containerName, CancellationToken cancellationToken);

    Task<Result<string>> CreateContainer(CreateContainerParameters containerParameters);

    Task<Result> CreateImageIfNotExists(string imageName, string version);
}
