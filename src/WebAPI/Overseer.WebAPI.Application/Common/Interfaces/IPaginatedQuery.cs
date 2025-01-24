namespace Overseer.WebAPI.Application.Common.Interfaces;

/// <summary>
///     Represents a contract for paginated queries, ensuring the presence of pagination parameters.
/// </summary>
public interface IPaginatedQuery
{
    /// <value>
    ///     Gets the page number for the query.
    /// </value>
    int PageNumber { get; }

    /// <value>
    ///     Gets the number of items per page for the query.
    /// </value>
    int PageSize { get; }
}
