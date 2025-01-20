using Overseer.WebAPI.Domain.Common;

namespace Overseer.WebAPI.Domain.Entities.VersioningContainers;

public class VersioningContainerVersionTag : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public Guid ContainerId { get; set; }
    
    public VersioningContainer Container { get; set; } = null!;

    public List<VersioningContainerVersionTagValue> Values { get; set; } = [];
}