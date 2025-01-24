using Overseer.WebAPI.Endpoints.Containers;
using Overseer.WebAPI.Endpoints.Projects;

namespace Overseer.WebAPI.Endpoints;

internal static class EndpointMapping
{
    internal static void MapEndpoints(this WebApplication app)
    {
        MapProjectEndpoints(app);
        MapContainerEndpoints(app);
    }

    private static void MapProjectEndpoints(WebApplication app)
    {
        RouteGroupBuilder projectGroup = ProjectEndpointGroup.MapGroup(app);
        CreateProject.MapEndpoint(projectGroup);
        GetProject.MapEndpoint(projectGroup);
        GetProjectsWithPagination.MapEndpoint(projectGroup);
        UpdateProject.MapEndpoint(projectGroup);
        DeleteProject.MapEndpoint(projectGroup);
    }

    private static void MapContainerEndpoints(WebApplication app)
    {
        RouteGroupBuilder containerGroup = ContainerEndpointGroup.MapGroup(app);
        GetContainersWithPagination.MapEndpoint(containerGroup);
        UpdateContainer.MapEndpoint(containerGroup);
        DeleteContainer.MapEndpoint(containerGroup);
    }
}
