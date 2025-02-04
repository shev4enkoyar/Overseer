using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;
using Overseer.WebAPI.Domain.Enums;

namespace Overseer.WebAPI.Application.Containers.Commands.CreateContainer;

public record CreateContainerCommand(
    Guid ProjectId,
    string Name,
    string? Description = null) : ICommand<Guid>;

internal sealed class CreateContainerCommandHandler(
    IProjectRepository projectRepository,
    IContainerRepository containerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateContainerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateContainerCommand request, CancellationToken cancellationToken)
    {
        Option<Project> project = await
            projectRepository.GetProjectWithoutTrackingAsync(request.ProjectId, cancellationToken);

        if (project.IsNone)
        {
            return Result<Guid>.Failure(new NotFoundException(nameof(Project), request.ProjectId));
        }

        var createdContainer =
            Container.Create(project.Value.Id, request.Name, ContainerType.SimpleS3, request.Description);

        containerRepository.AddContainer(createdContainer);

        Result<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

        return saveResult.Map(_ => createdContainer.Id, Result<Guid>.Failure);
    }
}
