using MediatR;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities.VersioningContainers;
using Unit = LanguageExt.Unit;

namespace Overseer.WebAPI.Application.VersioningContainers.Versions.Commands.CreateVersion;

public record CreateVersionCommand(
    Guid ContainerId,
    string Name,
    string? Description = null) : IRequest<Either<Exception, int>>;

internal sealed class CreateVersionCommandHandler(IVersioningContainerRepository versioningContainerRepository)
    : IRequestHandler<CreateVersionCommand, Either<Exception, int>>
{
    public async Task<Either<Exception, int>> Handle(CreateVersionCommand request, CancellationToken cancellationToken)
    {
        var newVersion = VersioningContainerVersion.Create(request.ContainerId, request.Name, request.Description);

        Either<Exception, Unit> versionCreatingResult = await versioningContainerRepository
            .CreateVersionAsync(newVersion, cancellationToken);

        return versionCreatingResult.Map(_ => newVersion.Id);
    }
}
