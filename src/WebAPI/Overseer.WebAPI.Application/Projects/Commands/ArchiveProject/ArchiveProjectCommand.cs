using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.ArchiveProject;

public record ArchiveProjectCommand(Guid ProjectId) : ICommand;

internal sealed class ArchiveProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveProjectCommand>
{
    public async Task<Result> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        return await projectOption.MapAsync(
            async project =>
            {
                project.Archive();

                Result<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

                return saveResult.Map(Result.Success, Result.Failure);
            },
            () => Result.Failure(new NotFoundException(nameof(Project), request.ProjectId)));
    }
}
