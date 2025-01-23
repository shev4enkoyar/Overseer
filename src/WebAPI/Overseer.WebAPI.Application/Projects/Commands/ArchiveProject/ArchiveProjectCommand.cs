using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.ArchiveProject;

public record ArchiveProjectCommand(Guid ProjectId) : ICommand;

internal sealed class ArchiveProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveProjectCommand>
{
    public async Task<Either<Error, Unit>> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken) =>
        await TryAsync(async () =>
        {
            Option<Project> projectOption =
                await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);
            return await projectOption.MatchAsync(async project =>
            {
                project.Archive();
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Right<Error, Unit>(Unit.Default);
            }, () => Left<Error, Unit>(Error.New(new NotFoundException(nameof(Project), request.ProjectId))));
        }).Match(eitherResult => eitherResult, exception => Left<Error, Unit>(exception));
}
