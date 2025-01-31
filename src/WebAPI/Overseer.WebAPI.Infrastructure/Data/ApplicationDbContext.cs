using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Overseer.FluentExtensions.Error;
using Overseer.FluentExtensions.Result;
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

    public async Task<Result<int>> TrySaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Create(new DatabaseException("Error saving changes to the database", e));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
