using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.ArchiveProject;

public record ArchiveProjectCommand(Guid ProjectId) : ICommand;

internal sealed class ArchiveProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveProjectCommand>
{
    public async Task<Fin<Unit>> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        return await projectOption.MatchAsync(
            async project =>
            {
                project.Archive();

                Fin<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

                return saveResult.Match(_ => Fin<Unit>.Succ(Unit.Default), Fin<Unit>.Fail);
            },
            () => Fin<Unit>.Fail(new NotFoundException(nameof(Project), request.ProjectId)));
    }
}
