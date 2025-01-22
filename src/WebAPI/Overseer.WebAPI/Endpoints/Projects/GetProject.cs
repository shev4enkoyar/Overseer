using MediatR;
using Overseer.WebAPI.Application.Projects.Queries.GetProject;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

public abstract class GetProject : IEndpoint
{
    public const string EndpointName = "GetProject";

    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("{id:guid}",
                async (Guid id, ISender sender, IApiErrorHandler errorHandler) =>
                {
                    var response = await sender.Send(new GetProjectQuery(id));
                    return response.Match(Left: errorHandler.Handle, Right: Results.Ok);
                })
            .WithSummary("Get project")
            .WithDescription(
                "Retrieves details of the project with the specified ID, returning the project data upon success.")
            .Produces<ProjectDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(EndpointName);
    }
}