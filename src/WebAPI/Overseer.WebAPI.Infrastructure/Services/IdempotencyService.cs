using Microsoft.Extensions.Caching.Hybrid;
using Overseer.WebAPI.Application.Common.Abstractions.Idempotency;

namespace Overseer.WebAPI.Infrastructure.Services;

public class IdempotencyService(HybridCache cache) : IIdempotencyService
{
    private const string CacheKeyPrefix = "idempotency";

    public async Task<T> GetRequestOrCreateAsync<T>(Guid requestId, string requestName,
        Func<CancellationToken, ValueTask<T>> factory) =>
        await cache.GetOrCreateAsync<T>($"{CacheKeyPrefix}:{requestName}:{requestId}", factory.Invoke);
}
