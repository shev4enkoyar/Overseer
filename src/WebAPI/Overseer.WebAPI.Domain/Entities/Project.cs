using Overseer.WebAPI.Domain.Common;

namespace Overseer.WebAPI.Domain.Entities;

public class Project : BaseEntity<Guid>
{
    public string Name { get; private set; } = null!;
    
    public string? Description { get; private set; }
    
    public bool IsArchived { get; private set; }

    public List<Container> Containers { get; private  set; } = [];

    public void Archive()
    {
        IsArchived = true;
    }

    public void UpdateBaseInfo(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
    
    public static Project Create(string name, string? description = null)
    {
        return new Project
        {
            Name = name,
            Description = description
        };
    }
}