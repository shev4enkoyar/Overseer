using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IProjectRepository
{
    Task AddProjectAsync(Project project, CancellationToken cancellationToken);
    
    Task DeleteProjectAsync(Project project, CancellationToken cancellationToken);

    Task<Option<Project>> GetProjectWithoutTrackingAsync(Guid id, CancellationToken cancellationToken);
    Task<Option<Project>> GetProjectAsync(Guid id, CancellationToken cancellationToken);
}