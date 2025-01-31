using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Application.Common.Mapping;
using Overseer.WebAPI.Domain.Abstractions;

namespace Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;

public record GetProjectsWithPaginationQuery(
    int PageNumber,
    int PageSize) : IQuery<PaginatedList<ProjectBriefDto>>, IPaginatedQuery;

internal sealed class GetProjectsWithPaginationQueryHandler(IProjectRepository projectRepository)
    : IQueryHandler<GetProjectsWithPaginationQuery, PaginatedList<ProjectBriefDto>>
{
    public async Task<Result<PaginatedList<ProjectBriefDto>>> Handle(GetProjectsWithPaginationQuery request,
        CancellationToken cancellationToken) =>
        await projectRepository.GetProjectsWithPaginationAsync(
                request.PageNumber, request.PageSize,
                cancellationToken)
            .ToProjectBriefDtoPaginatedListAsync();
}
