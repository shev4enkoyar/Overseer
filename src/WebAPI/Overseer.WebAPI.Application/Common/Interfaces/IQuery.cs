using MediatR;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IQuery<T> : IRequest<Result<T>>;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>;
