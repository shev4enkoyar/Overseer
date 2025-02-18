namespace Overseer.WebAPI.Domain.Abstractions;

public class PaginatedList<T> where T : class
{
    private PaginatedList()
    {
    }

    public IReadOnlyCollection<T> Items { get; private set; } = null!;

    public int PageNumber { get; private set; }

    public int TotalPages { get; private set; }

    public int TotalCount { get; private set; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static PaginatedList<T> Create(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize) =>
        new()
        {
            PageNumber = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            TotalCount = count,
            Items = items
        };

    public static PaginatedList<T> CreateRaw(IReadOnlyCollection<T> items, int count, int pageNumber, int totalPages) =>
        new()
        {
            PageNumber = pageNumber,
            TotalPages = totalPages,
            TotalCount = count,
            Items = items
        };
}
