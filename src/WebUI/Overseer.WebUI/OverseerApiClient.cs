using System.Diagnostics.CodeAnalysis;

namespace Overseer.WebUI;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
public sealed class OverseerApiClient(HttpClient httpClient)
{
    public async Task<PaginatedList<ProjectBriefDto>> GetPaginatedProjectsAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        PaginatedList<ProjectBriefDto>? some =
            await httpClient.GetFromJsonAsync<PaginatedList<ProjectBriefDto>>(
                $"projects?pageNumber={pageNumber}&pageSize={pageSize}", cancellationToken) ??
            throw new InvalidOperationException();
        return some;
    }
}

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
public class PaginatedList<T> where T : class
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public int PageNumber { get; init; }

    public int TotalPages { get; init; }

    public int TotalCount { get; init; }

    public bool HasPreviousPage { get; init; }

    public bool HasNextPage { get; init; }
}

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
public class ProjectBriefDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string? Description { get; init; }
}
