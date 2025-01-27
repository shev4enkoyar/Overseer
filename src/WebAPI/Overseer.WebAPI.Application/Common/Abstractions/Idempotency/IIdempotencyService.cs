namespace Overseer.WebAPI.Application.Common.Abstractions.Idempotency;

public interface IIdempotencyService
{
    Task<T> GetRequestOrCreateAsync<T>(Guid requestId, string requestName,
        Func<CancellationToken, ValueTask<T>> factory);
}
