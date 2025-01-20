using Overseer.WebAPI.Application.Projects.Queries.GetProject;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Common.Mapping;

internal static class ProjectExtensionMapping
{
    internal static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description
        };
    }
}