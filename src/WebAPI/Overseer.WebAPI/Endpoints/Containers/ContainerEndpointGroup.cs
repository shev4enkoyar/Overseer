using Overseer.WebAPI.Infrastructure;

namespace Overseer.WebAPI.Endpoints.Containers;

internal abstract class ContainerEndpointGroup : IEndpointGroup
{
    public static RouteGroupBuilder MapGroup(WebApplication app) =>
        app.MapGroup("api/containers")
            .WithTags(EndpointTags.Containers)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
}
