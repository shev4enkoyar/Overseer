using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : ICommand;

internal sealed class DeleteProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Fin<Unit>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        return await projectOption.MatchAsync(
            async project =>
            {
                await projectRepository.DeleteProjectAsync(project, cancellationToken);

                Fin<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

                return saveResult.Match(_ => Fin<Unit>.Succ(Unit.Default), Fin<Unit>.Fail);
            },
            () => Fin<Unit>.Fail(new NotFoundException(nameof(Project), request.ProjectId)));
    }
}
