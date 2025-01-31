using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : ICommand;

internal sealed class DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        return await projectOption.MapAsync(
            async project =>
            {
                await projectRepository.DeleteProjectAsync(project, cancellationToken);

                Result<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

                return saveResult.Map(Result.Success, Result.Failure);
            },
            () => Result.Failure(new NotFoundException(nameof(Project), request.ProjectId)));
    }
}
