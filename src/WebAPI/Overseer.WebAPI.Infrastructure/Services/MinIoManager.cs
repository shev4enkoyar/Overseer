using Docker.DotNet.Models;
using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Infrastructure.Services;

public class MinIoManager(IDockerClientManager dockerClientManager) : IMinIoManager
{
    public async Task<Result<string>> CreateNewMinIoContainerAsync(string containerName, string userName,
        string password, CancellationToken cancellationToken)
    {
        const string imageName = "minio/minio";

        var envVariables = new List<string>
        {
            $"MINIO_ROOT_USER={userName}",
            $"MINIO_ROOT_PASSWORD={password}"
        };

        Result downloadImageResult = await dockerClientManager.CreateImageIfNotExists(imageName, "latest");
        if (downloadImageResult.IsFailure)
        {
            return Result<string>.Failure(downloadImageResult.Error);
        }

        var config = new CreateContainerParameters
        {
            Image = imageName,
            Name = containerName,
            Env = envVariables,
            ExposedPorts = new Dictionary<string, EmptyStruct>
            {
                { "9000/tcp", new EmptyStruct() },
                { "9001/tcp", new EmptyStruct() }
            },
            Labels = new Dictionary<string, string>
            {
                { "traefik.enable", "true" },
                { "traefik.docker.network", "proxynet" },
                { "traefik.constraint-label", "proxynet" },

                { $"traefik.http.routers.minio-{containerName}.service", $"minio-{containerName}" },
                { $"traefik.http.routers.minio-{containerName}.rule", $"Host(`storage.{containerName}.localhost`)" },
                { $"traefik.http.services.minio-{containerName}.loadbalancer.server.port", "9000" },

                { $"traefik.http.routers.minio-{containerName}-console.service", $"minio-{containerName}-console" },
                {
                    $"traefik.http.routers.minio-{containerName}-console.rule",
                    $"Host(`stash.{containerName}.localhost`)"
                },
                { $"traefik.http.services.minio-{containerName}-console.loadbalancer.server.port", "9001" }
            },
            HostConfig = new HostConfig
            {
                NetworkMode = "proxynet",
                Mounts =
                [
                    new Mount
                    {
                        Target = "/data",
                        Source = $"minio_{containerName}_data_volume",
                        Type = "volume"
                    }
                ]
            },
            Cmd = ["server", "/data", "--console-address", ":9001"]
        };


        Result<string> createContainerResult = await dockerClientManager.CreateContainer(config);

        if (createContainerResult.IsFailure)
        {
            return Result<string>.Failure(createContainerResult.Error);
        }

        Result startContainerResult =
            await dockerClientManager.StartContainerById(createContainerResult.Value, cancellationToken);

        return startContainerResult.IsFailure
            ? Result<string>.Failure(createContainerResult.Error)
            : Result<string>.Success(createContainerResult.Value);
    }
}
