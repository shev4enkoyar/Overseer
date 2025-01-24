using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Application.Containers.Commands.UpdateContainer;

public record UpdateContainerCommand(
    Guid ContainerId,
    string Name,
    string? Description = null) : ICommand;

internal sealed class UpdateContainerCommandHandler : ICommandHandler<UpdateContainerCommand>
{
    public Task<Either<Error, Unit>> Handle(UpdateContainerCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
