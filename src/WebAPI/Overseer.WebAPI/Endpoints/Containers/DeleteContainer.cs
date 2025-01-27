using System.ComponentModel;
using LanguageExt;
using MediatR;
using Overseer.WebAPI.Application.Containers.Commands.DeleteContainer;
using Overseer.WebAPI.Infrastructure;
using Unit = LanguageExt.Unit;

namespace Overseer.WebAPI.Endpoints.Containers;

internal abstract class DeleteContainer : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapDelete("{id:guid}", static async (
                [Description("The unique identifier of the container to be deleted.")]
                Guid id,
                ISender sender,
                IApiErrorHandler errorHandler) =>
            {
                Fin<Unit> response = await sender.Send(new DeleteContainerCommand(id));
                return response.Match<IResult>(static _ => Results.NoContent(), errorHandler.Handle);
            })
            .WithSummary("Delete container")
            .WithDescription(
                "Removes the container identified by its unique ID. " +
                "If the container with the specified ID does not exist, a 404 Not Found response will be returned.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
