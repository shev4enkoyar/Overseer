using Microsoft.AspNetCore.Components;
using Overseer.WebUI.Services;

namespace Overseer.WebUI.Components.Pages;

public partial class Projects(OverseerApiClient client, NavigationService navigationService) : ComponentBase
{
#pragma warning disable IDE0052
    private PaginatedList<ProjectBriefDto> ProjectPage { get; set; } = new();
#pragma warning restore IDE0052
    protected override async Task OnInitializedAsync() => ProjectPage = await client.GetPaginatedProjectsAsync(1, 10);

    private void OnProjectItemClick(Guid projectId) => navigationService.NavigateToProjectContainers(projectId);
}
