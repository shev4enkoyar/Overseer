using Overseer.WebAPI.Domain.Entities.VersioningContainers;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IVersioningContainerRepository
{
    public Task<Result> CreateVersionAsync(VersioningContainerVersion version,
        CancellationToken cancellationToken);

    public Task UpdateVersionAsync(VersioningContainerVersion version, CancellationToken cancellationToken);

    public Task DeleteVersionAsync(VersioningContainerVersion version, CancellationToken cancellationToken);

    public Task<VersioningContainerVersion> GetVersionByIdAsync(int versionId, CancellationToken cancellationToken);

    public Task CreateTagAsync(VersioningContainerVersionTag tag, CancellationToken cancellationToken);

    public Task UpdateTagAsync(VersioningContainerVersionTag tag, CancellationToken cancellationToken);

    public Task DeleteTagAsync(VersioningContainerVersionTag tag, CancellationToken cancellationToken);

    public Task<VersioningContainerVersionTag[]> GetVersionContainerTagsAsync(VersioningContainer container,
        CancellationToken cancellationToken);
}
