using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Application.Common.Mapping;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Queries.GetProject;

public record GetProjectQuery(Guid Id) : IQuery<ProjectDto>;

internal sealed class GetProjectQueryHandler(IProjectRepository projectRepository)
    : IQueryHandler<GetProjectQuery, ProjectDto>
{
    public async Task<Either<Error, ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken) =>
        await TryOptionAsync(async () =>
                await projectRepository.GetProjectWithoutTrackingAsync(request.Id, cancellationToken))
            .Match(project => Right<Error, ProjectDto>(project.ToDto()),
                () => Left<Error, ProjectDto>(Error.New(new NotFoundException(nameof(Project), request.Id))),
                exception => Left<Error, ProjectDto>(exception));
}
