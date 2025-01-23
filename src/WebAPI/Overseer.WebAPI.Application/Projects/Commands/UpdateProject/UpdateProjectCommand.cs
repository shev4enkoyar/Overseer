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
    public async Task<Either<Error, Unit>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken) =>
        await TryAsync(async () =>
        {
            Option<Project> projectOption =
                await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);
            return await projectOption.MatchAsync(async project =>
            {
                project.UpdateBaseInfo(request.Name, request.Description);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Right<Error, Unit>(Unit.Default);
            }, () => Left<Error, Unit>(Error.New(new NotFoundException(nameof(Project), request.ProjectId))));
        }).Match(eitherResult => eitherResult, exception => Left<Error, Unit>(exception));
}
