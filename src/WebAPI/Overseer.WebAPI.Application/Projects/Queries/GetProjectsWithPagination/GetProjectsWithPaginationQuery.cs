using LanguageExt.SomeHelp;
using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Application.Common.Mapping;
using Overseer.WebAPI.Domain.Abstractions;

namespace Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;

public record GetProjectsWithPaginationQuery(
    int PageNumber,
    int PageSize) : IQuery<PaginatedList<ProjectBriefDto>>;

internal class GetProjectsWithPaginationQueryHandler(IProjectRepository projectRepository) 
    : IQueryHandler<GetProjectsWithPaginationQuery, PaginatedList<ProjectBriefDto>>
{
    public async Task<Either<Error, PaginatedList<ProjectBriefDto>>> Handle(GetProjectsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var error = Error.New(new ValidationException());
        var some = error.ToSome();
        var left = (Either<Error, PaginatedList<ProjectBriefDto>>)LeftUnsafe(error);
        
        return await TryAsync(async () => 
                await projectRepository.GetProjectsWithPaginationAsync(request.PageNumber, request.PageSize, 
                    cancellationToken)
                    .ToProjectBriefDtoPaginatedListAsync())
            .ToEither();
    }
}