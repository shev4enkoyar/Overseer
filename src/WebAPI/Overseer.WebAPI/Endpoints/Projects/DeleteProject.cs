using System.ComponentModel;
using LanguageExt;
using MediatR;
using Overseer.WebAPI.Application.Projects.Commands.DeleteProject;
using Overseer.WebAPI.Infrastructure;
using Unit = LanguageExt.Unit;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class DeleteProject : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) => routeGroupBuilder.MapDelete("{id:guid}",
            static async ([Description("The unique identifier of the project to be deleted.")] Guid id, ISender sender,
                IApiErrorHandler errorHandler) =>
            {
                Fin<Unit> response = await sender.Send(new DeleteProjectCommand(id));
                return response.Match<IResult>(static _ => Results.NoContent(), errorHandler.Handle);
            })
        .WithSummary("Delete project")
        .WithDescription(
            "Removes the project identified by its unique ID. " +
            "If the project with the specified ID does not exist, a 404 Not Found response will be returned.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
}
