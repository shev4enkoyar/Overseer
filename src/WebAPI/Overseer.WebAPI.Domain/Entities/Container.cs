using Overseer.WebAPI.Domain.Common;
using Overseer.WebAPI.Domain.Enums;

namespace Overseer.WebAPI.Domain.Entities;

public class Container : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }

    public Guid ProjectId { get; set; }
    
    public Project Project { get; set; } = null!;
    
    public ContainerType Type { get; set; }
    
    public Guid TypedContainerId { get; set; }
}