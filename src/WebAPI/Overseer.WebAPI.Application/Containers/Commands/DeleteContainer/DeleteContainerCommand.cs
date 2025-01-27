using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Application.Containers.Commands.DeleteContainer;

public record DeleteContainerCommand(Guid ContainerId) : ICommand;

internal sealed class DeleteContainerCommandHandler : ICommandHandler<DeleteContainerCommand>
{
    public Task<Fin<Unit>> Handle(DeleteContainerCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
