using Overseer.WebAPI.Endpoints.Projects;

namespace Overseer.WebAPI.Infrastructure;

internal static class EndpointMapping
{
    internal static void MapEndpoints(this WebApplication app)
    {
        var projectGroup = ProjectEndpointGroup.MapGroup(app);
        CreateProject.MapEndpoint(projectGroup);
        GetProject.MapEndpoint(projectGroup);
    }
}