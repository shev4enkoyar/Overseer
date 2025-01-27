using Overseer.WebAPI.Application.Common.Abstractions.Idempotency;
using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    Guid RequestId,
    string Name,
    string? Description = null) : IdempotentCommand<Guid>(RequestId);

internal sealed class CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProjectCommand, Guid>
{
    public async Task<Fin<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
#pragma warning disable CA2201
        return Error.New(new NotFoundException("name", "some"));
#pragma warning restore CA2201
#pragma warning disable CS0162 // Unreachable code detected
        var project = Project.Create(request.Name, request.Description);

        await projectRepository.AddProjectAsync(project, cancellationToken);

        Fin<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

        return saveResult.Match(_ => Fin<Guid>.Succ(project.Id), Fin<Guid>.Fail);
#pragma warning restore CS0162 // Unreachable code detected
    }
}
