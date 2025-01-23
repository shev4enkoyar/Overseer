using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class ProjectEndpointGroup : IEndpointGroup
{
    public static RouteGroupBuilder MapGroup(WebApplication app) => app.MapGroup("api/projects")
        .WithTags(EndpointTags.Projects)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
}
