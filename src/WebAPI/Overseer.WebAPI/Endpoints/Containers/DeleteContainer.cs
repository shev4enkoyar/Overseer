using System.ComponentModel;
using MediatR;
using Overseer.FluentExtensions.Result;
using Overseer.WebAPI.Application.Containers.Commands.DeleteContainer;
using Overseer.WebAPI.Infrastructure;

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
                Result response = await sender.Send(new DeleteContainerCommand(id));
                return response.Map(static () => Results.NoContent(), errorHandler.Handle);
            })
            .WithSummary("Delete container")
            .WithDescription(
                "Removes the container identified by its unique ID. " +
                "If the container with the specified ID does not exist, a 404 Not Found response will be returned.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
