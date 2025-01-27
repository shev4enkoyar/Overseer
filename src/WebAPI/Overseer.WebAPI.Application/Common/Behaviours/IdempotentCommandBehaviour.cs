using MediatR;
using Overseer.WebAPI.Application.Common.Abstractions.Idempotency;

namespace Overseer.WebAPI.Application.Common.Behaviours;

public sealed class IdempotentCommandBehaviour<TRequest, TResponse>(IIdempotencyService idempotencyService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IdempotentCommandBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) =>
        await idempotencyService.GetRequestOrCreateAsync<TResponse>(request.RequestId, typeof(TRequest).Name,
            async _ => await next());
}
