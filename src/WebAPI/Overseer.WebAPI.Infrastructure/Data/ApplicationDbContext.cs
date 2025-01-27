using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Overseer.WebAPI.Application.Common.Exceptions;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities;
using Overseer.WebAPI.Domain.Entities.VersioningContainers;

namespace Overseer.WebAPI.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<Container> Containers => Set<Container>();

    public DbSet<VersioningContainer> VersioningContainers => Set<VersioningContainer>();

    public DbSet<VersioningContainerVersion> VersioningContainerVersions => Set<VersioningContainerVersion>();

    public DbSet<VersioningContainerVersionTag> VersioningContainerVersionTags => Set<VersioningContainerVersionTag>();

    public DbSet<VersioningContainerVersionTagValue> VersioningContainerVersionTagValues =>
        Set<VersioningContainerVersionTagValue>();

    public async Task<Fin<int>> TrySaveChangesAsync(CancellationToken cancellationToken) =>
        await TryAsync(async () => await SaveChangesAsync(cancellationToken))
            .Match(
                Fin<int>.Succ,
                ex => Fin<int>.Fail(new DatabaseException("Error saving changes to the database", ex))
            );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
