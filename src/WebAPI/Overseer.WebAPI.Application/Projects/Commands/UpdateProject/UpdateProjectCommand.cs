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
    public async Task<Fin<Unit>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Option<Project> projectOption = await projectRepository.GetProjectAsync(request.ProjectId, cancellationToken);

        return await projectOption.MatchAsync(
            async project =>
            {
                project.UpdateBaseInfo(request.Name, request.Description);

                Fin<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

                return saveResult.Match(_ => Fin<Unit>.Succ(Unit.Default), Fin<Unit>.Fail);
            },
            () => Fin<Unit>.Fail(new NotFoundException(nameof(Project), request.ProjectId)));
    }
}
