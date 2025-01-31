using MediatR;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities.VersioningContainers;

namespace Overseer.WebAPI.Application.VersioningContainers.Versions.Commands.CreateVersion;

public record CreateVersionCommand(
    Guid ContainerId,
    string Name,
    string? Description = null) : IRequest<Result<int>>;

internal sealed class CreateVersionCommandHandler(IVersioningContainerRepository versioningContainerRepository)
    : IRequestHandler<CreateVersionCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateVersionCommand request, CancellationToken cancellationToken)
    {
        var newVersion = VersioningContainerVersion.Create(request.ContainerId, request.Name, request.Description);

        Result versionCreatingResult = await versioningContainerRepository
            .CreateVersionAsync(newVersion, cancellationToken);

        return versionCreatingResult.Map(() => Result<int>.Success(newVersion.Id), Result<int>.Failure);
    }
}
