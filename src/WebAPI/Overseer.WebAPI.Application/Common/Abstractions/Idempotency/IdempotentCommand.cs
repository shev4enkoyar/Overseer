using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Application.Common.Abstractions.Idempotency;

public abstract record IdempotentCommandBase(Guid RequestId);

public abstract record IdempotentCommand<T>(Guid RequestId) : IdempotentCommandBase(RequestId), ICommand<T>;
