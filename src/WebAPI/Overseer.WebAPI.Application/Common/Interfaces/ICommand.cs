using MediatR;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface ICommand : IRequest<Result>;

public interface ICommand<T> : IRequest<Result<T>>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;

public interface
    ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>>
    where TCommand : ICommand<TResult>;
