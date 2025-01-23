namespace Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;

public class ProjectBriefDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
