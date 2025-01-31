using System.ComponentModel;
using MediatR;
using Overseer.FluentExtensions.Result;
using Overseer.WebAPI.Application.Projects.Commands.UpdateProject;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class UpdateProject : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapPut("{id:guid}",
                static async ([Description("The unique identifier of the project to be updated.")] Guid id,
                    UpdateProjectRequest request, ISender sender, IApiErrorHandler errorHandler) =>
                {
                    Result response = await sender.Send(new UpdateProjectCommand(id, request.Name,
                        request.Description));
                    return response.Map(static () => Results.NoContent(), errorHandler.Handle);
                })
            .WithSummary("Update project")
            .WithDescription(
                "Updates the name and description of an existing project identified by its unique ID. " +
                "If the project with the specified ID does not exist, a 404 Not Found response will be returned.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
}

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed record UpdateProjectRequest(
    [property: Description("The name of the project to update. " +
                           "This field is required and should be a non-empty string.")]
    string Name,
    [property: Description("The optional description of the project. " +
                           "This field can provide additional details about the project.")]
    string? Description = null);
