using LanguageExt;
using LanguageExt.Common;
using MediatR;
using Overseer.WebAPI.Application.Projects.Commands.CreateProject;
using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class CreateProject : IEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder routeGroupBuilder) =>
        routeGroupBuilder.MapPost("",
                static async (CreateProjectRequest request, ISender sender, IApiErrorHandler errorHandler) =>
                {
                    Either<Error, Guid> response =
                        await sender.Send(new CreateProjectCommand(request.Name, request.Description));

                    return response.Match(
                        Left: errorHandler.Handle,
                        Right: static projectId =>
                            Results.CreatedAtRoute(GetProject.EndpointName, new { id = projectId }, projectId));
                })
            .WithSummary("Create project")
            .WithDescription(
                "Creates a new project with the specified name and description, returning the ID of the created project upon success.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
}

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed record CreateProjectRequest(string Name, string? Description = null);
