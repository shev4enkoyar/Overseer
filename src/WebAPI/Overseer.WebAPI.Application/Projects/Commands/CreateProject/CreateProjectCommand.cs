using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    string Name,
    string? Description = null) : ICommand<Guid>;

internal class CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateProjectCommand, Guid>
{
    public async Task<Either<Error, Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        return await TryAsync(async () =>
        {
            var project = Project.Create(request.Name, request.Description);

            await projectRepository.AddProjectAsync(project, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return project.Id;
        }).ToEither();
    }
}