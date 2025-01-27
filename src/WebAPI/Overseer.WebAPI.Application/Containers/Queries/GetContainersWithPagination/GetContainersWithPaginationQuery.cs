using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Abstractions;

namespace Overseer.WebAPI.Application.Containers.Queries.GetContainersWithPagination;

public record GetContainersWithPaginationQuery(
    int PageNumber,
    int PageSize) : IQuery<PaginatedList<ContainerBriefDto>>, IPaginatedQuery;

internal sealed class GetContainersWithPaginationQueryHandler
    : IQueryHandler<GetContainersWithPaginationQuery, PaginatedList<ContainerBriefDto>>
{
    public Task<Fin<PaginatedList<ContainerBriefDto>>> Handle(GetContainersWithPaginationQuery request,
        CancellationToken cancellationToken) => throw new NotImplementedException();
}
