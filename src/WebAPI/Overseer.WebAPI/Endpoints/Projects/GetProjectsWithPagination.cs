using MediatR;
using Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

public class GetProjectsWithPagination : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("", async (ISender sender, IApiErrorHandler errorHandler,int pageNumber = 1, int pageSize = 10) =>
        {
            var response = await sender.Send(new GetProjectsWithPaginationQuery(pageNumber, pageSize));
            return response.Match(Left: errorHandler.Handle, Right: Results.Ok);
        });
    }
}