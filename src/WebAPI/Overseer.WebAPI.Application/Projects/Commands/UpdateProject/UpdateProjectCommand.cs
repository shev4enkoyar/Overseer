using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(
    Guid ProjectId,
    string Name,
    string? Description = null) : ICommand;

internal sealed class UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProjectCommand>
{
    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        return await projectOption.MapAsync(async project =>
        {
            project.UpdateBaseInfo(request.Name, request.Description);

            Result<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

            return saveResult.Map(_ => Result.Success(), Result.Failure);
        }, () => Result.Failure(new NotFoundException(nameof(Project), request.ProjectId)));
    }
}
