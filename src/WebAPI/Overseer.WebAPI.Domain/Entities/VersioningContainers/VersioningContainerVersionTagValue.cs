using Overseer.WebAPI.Domain.Common;

namespace Overseer.WebAPI.Domain.Entities.VersioningContainers;

public class VersioningContainerVersionTagValue : BaseEntity<int>
{
    public string Value { get; set; } = null!;

    public int TagId { get; set; }

    public bool IsDefault { get; set; }

    public VersioningContainerVersionTag Tag { get; set; } = null!;

    public ICollection<VersioningContainerVersion> Versions { get; private set; } = [];
}
