using Overseer.WebAPI.Application.Projects.Queries.GetProject;
using Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;
using Overseer.WebAPI.Domain.Abstractions;
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
    
    public static async Task<PaginatedList<ProjectBriefDto>> ToProjectBriefDtoPaginatedListAsync(this Task<PaginatedList<Project>> paginatedProjectsTask)
    {
        var paginatedProjects = await paginatedProjectsTask;
        var projectBriefDtos = paginatedProjects.Items
            .Select(project => new ProjectBriefDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            })
            .ToList();

        return PaginatedList<ProjectBriefDto>.CreateRaw(projectBriefDtos,
            paginatedProjects.TotalCount,
            paginatedProjects.PageNumber,
            paginatedProjects.TotalPages);
    }
}