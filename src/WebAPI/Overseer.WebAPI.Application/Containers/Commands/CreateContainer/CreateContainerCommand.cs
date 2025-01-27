using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Application.Containers.Commands.CreateContainer;

public record CreateContainerCommand(
    string Name,
    string? Description = null) : ICommand;

internal sealed class CreateContainerCommandHandler : ICommandHandler<CreateContainerCommand>
{
    public Task<Fin<Unit>> Handle(CreateContainerCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
