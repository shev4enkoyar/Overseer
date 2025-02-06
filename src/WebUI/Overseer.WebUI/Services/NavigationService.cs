using Microsoft.AspNetCore.Components;

namespace Overseer.WebUI.Services;

#pragma warning disable CA1515
public sealed class NavigationService(NavigationManager navigationManager)
#pragma warning restore CA1515
{
    // ReSharper disable once UnusedParameter.Global
#pragma warning disable IDE0060
    public void NavigateToProjectContainers(Guid projectId) => navigationManager.NavigateTo("/containers");
#pragma warning restore IDE0060
}
