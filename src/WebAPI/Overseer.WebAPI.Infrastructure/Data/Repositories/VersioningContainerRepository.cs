using LanguageExt;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Domain.Entities.VersioningContainers;

namespace Overseer.WebAPI.Infrastructure.Data.Repositories;

public class VersioningContainerRepository(ApplicationDbContext dbContext) : IVersioningContainerRepository
{
    public async Task<Either<Exception, Unit>> CreateVersionAsync(VersioningContainerVersion version, CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.VersioningContainerVersions.AddAsync(version, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Default;
        }
        catch (Exception e)
        {
            return Either<Exception, Unit>.Left(e);
        }
    }

    public async Task UpdateVersionAsync(VersioningContainerVersion version, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteVersionAsync(VersioningContainerVersion version, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<VersioningContainerVersion> GetVersionByIdAsync(int versionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task CreateTagAsync(VersioningContainerVersionTag tag, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTagAsync(VersioningContainerVersionTag tag, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteTagAsync(VersioningContainerVersionTag tag, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<VersioningContainerVersionTag[]> GetVersionContainerTagsAsync(VersioningContainer container, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}