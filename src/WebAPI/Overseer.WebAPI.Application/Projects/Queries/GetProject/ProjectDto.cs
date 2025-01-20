namespace Overseer.WebAPI.Application.Projects.Queries.GetProject;

public class ProjectDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string? Description { get; init; }
}