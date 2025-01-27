using MediatR;
using Unit = LanguageExt.Unit;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface ICommand : IRequest<Fin<Unit>>;

public interface ICommand<T> : IRequest<Fin<T>>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Fin<Unit>> where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, Fin<TResult>>
    where TCommand : ICommand<TResult>;
