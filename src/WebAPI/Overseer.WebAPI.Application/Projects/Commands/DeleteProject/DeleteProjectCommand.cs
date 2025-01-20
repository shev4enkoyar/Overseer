using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : ICommand;

internal class DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork) 
    : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Either<Error, Unit>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        return await TryAsync(async () =>
        {
            var projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);
            return await projectOption.MatchAsync(async project =>
            {
                await projectRepository.DeleteProjectAsync(project, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Right<Error, Unit>(Unit.Default);
            }, () => Left<Error, Unit>(Error.New(new NotFoundException(nameof(Project), request.ProjectId))));
        }).Match(eitherResult => eitherResult, exception => Left<Error, Unit>(exception));
    }
}