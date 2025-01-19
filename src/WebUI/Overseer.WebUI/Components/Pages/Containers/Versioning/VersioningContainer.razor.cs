using Microsoft.AspNetCore.Components;

namespace Overseer.WebUI.Components.Pages.Containers.Versioning;

public partial class VersioningContainer : ComponentBase
{
    private Version Version { get; set; }
    private bool IsFilterDialogVisible { get; set; } = false;
}

public class Version
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Number { get; set; }
}