using System.ComponentModel;
using MediatR;
using Overseer.FluentExtensions.Result;
using Overseer.WebAPI.Application.Containers.Commands.UpdateContainer;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Containers;

internal abstract class UpdateContainer : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapPut("{id:guid}",
                static async ([Description("The unique identifier of the container to be updated.")] Guid id,
                    UpdateContainerRequest request, ISender sender, IApiErrorHandler errorHandler) =>
                {
                    Result response = await sender.Send(new UpdateContainerCommand(id, request.Name,
                        request.Description));
                    return response.Map(static () => Results.NoContent(), errorHandler.Handle);
                })
            .WithSummary("Update container")
            .WithDescription(
                "Updates the name and description of an existing container identified by its unique ID. " +
                "If the container with the specified ID does not exist, a 404 Not Found response will be returned.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
}

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed record UpdateContainerRequest(
    [property: Description("The name of the container to update. " +
                           "This field is required and should be a non-empty string.")]
    string Name,
    [property: Description("The optional description of the container. " +
                           "This field can provide additional details about the container.")]
    string? Description = null);
