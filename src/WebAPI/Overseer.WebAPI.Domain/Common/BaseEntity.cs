namespace Overseer.WebAPI.Domain.Common;

public abstract class BaseEntity<TIdentifier>
{
    public TIdentifier Id { get; set; } = default!;
}