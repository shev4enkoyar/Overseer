using Overseer.WebAPI.Extensions;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Configuration;

namespace Overseer.WebAPI.Endpoints.Projects;

internal abstract class ProjectEndpointGroup : IEndpointGroup
{
    public static RouteGroupBuilder MapGroup(WebApplication app) => app.MapGroup("api/projects")
        .WithTags(EndpointTags.Projects)
        .WithRateLimiting(RateLimiterSettings.TokenPolicy)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
}
