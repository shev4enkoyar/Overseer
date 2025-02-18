using Overseer.WebAPI.Domain.Common;

namespace Overseer.WebAPI.Domain.Entities.VersioningContainers;

public class VersioningContainerVersion : BaseEntity<int>
{
    private VersioningContainerVersion()
    {
    }

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public string? Link { get; private set; }

    public Guid ContainerId { get; private set; }

    public VersioningContainer Container { get; private set; } = null!;

    public ICollection<VersioningContainerVersionTagValue> TagValues { get; private set; } = [];

    public static VersioningContainerVersion Create(Guid containerId, string name, string? description = null) =>
        new()
        {
            ContainerId = containerId,
            Name = name,
            Description = description
        };

    public void SetVersionLink(string link)
    {
        if (string.IsNullOrWhiteSpace(Link))
        {
            Link = link;
        }
    }
}
