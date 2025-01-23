using Overseer.WebAPI.Domain.Common;

namespace Overseer.WebAPI.Domain.Entities.VersioningContainers;

public class VersioningContainer : BaseEntity<Guid>
{
    public ICollection<VersioningContainerVersion> Versions { get; init; } = [];

    public ICollection<VersioningContainerVersionTag> VersionTags { get; init; } = [];
}
