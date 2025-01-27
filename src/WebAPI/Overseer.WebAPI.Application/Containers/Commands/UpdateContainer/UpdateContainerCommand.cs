using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Application.Containers.Commands.UpdateContainer;

public record UpdateContainerCommand(
    Guid ContainerId,
    string Name,
    string? Description = null) : ICommand;

internal sealed class UpdateContainerCommandHandler : ICommandHandler<UpdateContainerCommand>
{
    public Task<Fin<Unit>> Handle(UpdateContainerCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
