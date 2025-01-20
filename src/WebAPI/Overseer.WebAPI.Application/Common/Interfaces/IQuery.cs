using MediatR;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IQuery<T> : IRequest<Either<Error, T>>;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Either<Error, TResult>> where TQuery : IQuery<TResult>;