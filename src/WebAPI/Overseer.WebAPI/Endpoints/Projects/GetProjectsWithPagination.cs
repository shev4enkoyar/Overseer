using System.ComponentModel;
using MediatR;
using Overseer.FluentExtensions.Result;
using Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;
using Overseer.WebAPI.Domain.Abstractions;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class GetProjectsWithPagination : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapGet("", static async (
                ISender sender,
                IApiErrorHandler errorHandler,
                [Description("The page number to retrieve. Must be greater than or equal to 1.")]
                int pageNumber = 1,
                [Description("The number of projects to include per page. Must be greater than or equal to 1.")]
                int pageSize = 10) =>
            {
                Result<PaginatedList<ProjectBriefDto>> response =
                    await sender.Send(new GetProjectsWithPaginationQuery(pageNumber, pageSize));
                return response.Map(Results.Ok, errorHandler.Handle);
            })
            .WithSummary("Get projects")
            .WithDescription(
                "Returns a paginated list of project summaries, allowing the client to specify the page number and page size. " +
                "If the request is successful, a paginated list of projects is returned. " +
                "In case of an error (e.g., invalid parameters), a problem response is returned.")
            .Produces<PaginatedList<ProjectBriefDto>>()
            .ProducesValidationProblem();
}
