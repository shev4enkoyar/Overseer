using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Application.Common.Mapping;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Queries.GetProject;

public record GetProjectQuery(Guid Id) : IQuery<ProjectDto>;

internal sealed class GetProjectQueryHandler(IProjectRepository projectRepository)
    : IQueryHandler<GetProjectQuery, ProjectDto>
{
    public async Task<Fin<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption =
            await projectRepository.GetProjectWithoutTrackingAsync(request.Id, cancellationToken);

        return projectOption.Match(
            project => Fin<ProjectDto>.Succ(project.ToDto()),
            () => Fin<ProjectDto>.Fail(Error.New(new NotFoundException(nameof(Project), request.Id))));
    }
}
