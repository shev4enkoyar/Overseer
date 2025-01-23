using LanguageExt;
using LanguageExt.Common;
using MediatR;
using Overseer.WebAPI.Application.Projects.Queries.GetProject;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class UpdateProject : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapPut("{id:guid}",
                async (Guid id, ISender sender, IApiErrorHandler errorHandler) =>
                {
                    Either<Error, ProjectDto> response = await sender.Send(new GetProjectQuery(id));
                    return response.Match(Left: errorHandler.Handle, Right: Results.Ok);
                })
            .WithSummary("Get project")
            .WithDescription(
                "Retrieves details of the project with the specified ID, returning the project data upon success.")
            .Produces<ProjectDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);
}
