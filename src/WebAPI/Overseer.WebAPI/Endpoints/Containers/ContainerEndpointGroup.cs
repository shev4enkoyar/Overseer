using Overseer.WebAPI.Extensions;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Configuration;

namespace Overseer.WebAPI.Endpoints.Containers;

internal abstract class ContainerEndpointGroup : IEndpointGroup
{
    public static RouteGroupBuilder MapGroup(WebApplication app) =>
        app.MapGroup("api/containers")
            .WithTags(EndpointTags.Containers)
            .WithRateLimiting(RateLimiterSettings.TokenPolicy)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
}
