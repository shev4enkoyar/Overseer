using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Overseer.WebAPI.Application.Projects.Commands.CreateProject;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class CreateProject : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("",
                async (CreateProjectRequest request, ISender sender, IApiErrorHandler errorHandler) =>
                {
                    var response = await sender.Send(new CreateProjectCommand(request.Name, request.Description));

                    return response.Match(
                        Left: errorHandler.Handle, 
                        Right: projectId => 
                            Results.CreatedAtRoute(GetProject.EndpointName, new { id = projectId }, projectId));
                })
            .WithSummary("Create project")
            .WithDescription(
                "Creates a new project with the specified name and description, returning the ID of the created project upon success.")
            .Produces<CreatedAtRoute<Guid>>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
internal record CreateProjectRequest(string Name, string? Description = null);