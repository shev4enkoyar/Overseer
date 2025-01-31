using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overseer.FluentExtensions.Result;
using Overseer.WebAPI.Application.Projects.Commands.CreateProject;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class CreateProject : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapPost("",
                static async (CreateProjectRequest request, [FromHeader(Name = "X-Idempotency-Key")] Guid requestId,
                    ISender sender, IApiErrorHandler errorHandler) =>
                {
                    Result<Guid> response =
                        await sender.Send(new CreateProjectCommand(requestId, request.Name, request.Description));

                    return response.Map(static projectId =>
                            Results.CreatedAtRoute(GetProject.EndpointName, new { id = projectId }, projectId),
                        errorHandler.Handle);
                })
            .WithSummary("Create project")
            .WithDescription(
                "Creates a new project with the specified name and description, returning the ID of the created project upon success.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
}

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed record CreateProjectRequest(
    [property: Description("The name of the project to create. " +
                           "This field is required and should be a non-empty string.")]
    string Name,
    [property: Description("The optional description of the project. " +
                           "This field can provide additional details about the project.")]
    string? Description = null);
