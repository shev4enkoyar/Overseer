using Overseer.FluentExtensions.Error;
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
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
#pragma warning disable CA2201
        var some = Error.Create(new NotFoundException("AAAAA", 1));
#pragma warning restore CA2201
        return some;
#pragma warning disable CS0162 // Unreachable code detected
        var project = Project.Create(request.Name, request.Description);

        await projectRepository.AddProjectAsync(project, cancellationToken);

        Result<int> saveResult = await unitOfWork.TrySaveChangesAsync(cancellationToken);

        return saveResult.Map(() => Result<Guid>.Success(project.Id), Result<Guid>.Failure);
#pragma warning restore CS0162 // Unreachable code detected
    }
}
