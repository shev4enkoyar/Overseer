using MediatR;
using Unit = LanguageExt.Unit;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface ICommand : IRequest<Either<Error, Unit>>;
public interface ICommand<T> : IRequest<Either<Error, T>>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Either<Error, Unit>> where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, Either<Error, TResult>> where TCommand : ICommand<TResult>;