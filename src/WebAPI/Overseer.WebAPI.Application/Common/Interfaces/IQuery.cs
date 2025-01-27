using MediatR;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IQuery<T> : IRequest<Fin<T>>;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Fin<TResult>>
    where TQuery : IQuery<TResult>;
