using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;
using Overseer.WebAPI.Application.Common.Exceptions;

namespace Overseer.WebAPI.Infrastructure.Services;

public class DockerClientManager : IDockerClientManager, IDisposable
{
    private readonly DockerClient _dockerClient = new DockerClientConfiguration(new Uri(GetDockerPipeName()))
        .CreateClient();

    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<Result> StartContainerById(string containerId, CancellationToken cancellationToken)
    {
        bool result = await _dockerClient.Containers.StartContainerAsync(containerId, null, cancellationToken);
        return result
            ? Result.Success()
            : Result.Failure(new ArgumentException($"Couldn't start container {containerId}"));
    }

    public async Task<Result> StartContainerByName(string containerName, CancellationToken cancellationToken)
    {
        Result<ContainerListResponse> containerFindResult = await GetContainerByName(containerName, cancellationToken);
        if (containerFindResult.IsFailure)
        {
            return Result.Failure(containerFindResult.Error);
        }

        return await StartContainerById(containerFindResult.Value.ID, cancellationToken);
    }

    public async Task<Result> StopContainerById(string containerId, CancellationToken cancellationToken)
    {
        bool result = await _dockerClient.Containers.StopContainerAsync(containerId, null, cancellationToken);
        return result
            ? Result.Success()
            : Result.Failure(new ArgumentException($"Couldn't stop container {containerId}"));
    }

    public async Task<Result> StopContainerByName(string containerName, CancellationToken cancellationToken)
    {
        Result<ContainerListResponse> containerFindResult = await GetContainerByName(containerName, cancellationToken);
        if (containerFindResult.IsFailure)
        {
            return Result.Failure(containerFindResult.Error);
        }

        return await StopContainerById(containerFindResult.Value.ID, cancellationToken);
    }

    public async Task<Result> RemoveContainerById(string containerId, CancellationToken cancellationToken)
    {
        try
        {
            await _dockerClient.Containers.RemoveContainerAsync(containerId, null, cancellationToken);
        }
        catch (Exception e)
        {
            Result.Failure(new ArgumentException($"Couldn't remove container {containerId}", e));
        }

        return Result.Success();
    }

    public async Task<Result> RemoveContainerByName(string containerName, CancellationToken cancellationToken)
    {
        Result<ContainerListResponse> containerFindResult = await GetContainerByName(containerName, cancellationToken);
        if (containerFindResult.IsFailure)
        {
            return Result.Failure(containerFindResult.Error);
        }

        return await RemoveContainerById(containerFindResult.Value.ID, cancellationToken);
    }

    public async Task<Result<string>> CreateContainer(CreateContainerParameters containerParameters)
    {
        CreateContainerResponse? response = await _dockerClient.Containers.CreateContainerAsync(containerParameters);
        return Result<string>.Success(response.ID);
    }

    public async Task<Result> CreateImageIfNotExists(string imageName, string version)
    {
        try
        {
            IList<ImagesListResponse>? images = await _dockerClient.Images
                .ListImagesAsync(new ImagesListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        {
                            "reference",
                            new Dictionary<string, bool>
                            {
                                { imageName, true }
                            }
                        }
                    }
                });
            if (!images.Any())
            {
                await _dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
                {
                    FromImage = imageName,
                    Tag = version
                }, null, new Progress<JSONMessage>());
            }
        }
        catch (Exception e)
        {
            return Result.Failure(e);
        }

        return Result.Success();
    }

    private async Task<Result<ContainerListResponse>> GetContainerByName(string containerName,
        CancellationToken cancellationToken)
    {
        IList<ContainerListResponse>? containers = await _dockerClient.Containers.ListContainersAsync(
            new ContainersListParameters
            {
                All = true // Включить остановленные контейнеры
            }, cancellationToken);

        ContainerListResponse? container = containers.FirstOrDefault(c => c.Names.Contains($"/{containerName}"));

        return container ?? Result<ContainerListResponse>
            .Failure(new NotFoundException(containerName, nameof(ContainerListResponse)));
    }

    private static string GetDockerPipeName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "npipe://./pipe/docker_engine";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "unix:/var/run/docker.sock";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "unix:///var/run/docker.sock";
        }

        throw new PlatformNotSupportedException("Unsupported OS platform");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _dockerClient.Dispose();
        }

        _isDisposed = true;
    }


    ~DockerClientManager() => Dispose(false);
}
