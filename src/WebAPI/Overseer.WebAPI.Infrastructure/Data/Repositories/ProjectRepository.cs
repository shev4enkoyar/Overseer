using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Abstractions;
using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Infrastructure.Data.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
{
    private static readonly Func<ApplicationDbContext, Guid, CancellationToken, Task<Project?>>
        FirstProjectWithoutTracking =
            EF.CompileAsyncQuery(
                (ApplicationDbContext context, Guid id, CancellationToken cancellationToken) => context
                    .Projects.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id)));

    private static readonly Func<ApplicationDbContext, Guid, CancellationToken, Task<Project?>> FirstProject =
        EF.CompileAsyncQuery(
            (ApplicationDbContext context, Guid id, CancellationToken cancellationToken) => context
                .Projects.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id)));

    public async Task AddProjectAsync(Project project, CancellationToken cancellationToken) =>
        await dbContext.Projects.AddAsync(project, cancellationToken);

    public Task DeleteProjectAsync(Project project, CancellationToken cancellationToken)
    {
        dbContext.Projects.Remove(project);
        return Task.CompletedTask;
    }

    public async Task<Option<Project>> GetProjectWithoutTrackingAsync(Guid id, CancellationToken cancellationToken) =>
        await FirstProjectWithoutTracking.Invoke(dbContext, id, cancellationToken);

    public async Task<Option<Project>> GetProjectAsync(Guid id, CancellationToken cancellationToken) =>
        await FirstProject.Invoke(dbContext, id, cancellationToken);

    public async Task<PaginatedList<Project>> GetProjectsWithPaginationAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        int count = await dbContext.Projects.CountAsync(cancellationToken);
        List<Project> filteredProjects = await dbContext.Projects
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);
        return PaginatedList<Project>.Create(filteredProjects, count, pageNumber, pageSize);
    }
}
