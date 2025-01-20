using Overseer.WebAPI.Domain.Common;

namespace Overseer.WebAPI.Domain.Entities.VersioningContainers;

public class VersioningContainer : BaseEntity<Guid>
{
    public List<VersioningContainerVersion> Versions { get; set; } = [];
    
    public List<VersioningContainerVersionTag> VersionTags { get; set; } = [];
}