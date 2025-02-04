using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Overseer.WebAPI.Application.Common.Abstractions.Idempotency;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Infrastructure.Data;
using Overseer.WebAPI.Infrastructure.Data.Repositories;
using Overseer.WebAPI.Infrastructure.Services;

namespace Overseer.WebAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDockerClientManager, DockerClientManager>();
        services.AddScoped<IMinIoManager, MinIoManager>(sp =>
            new MinIoManager(sp.GetRequiredService<IDockerClientManager>()));

        string? databaseConnectionString = configuration["ConnectionStings:Database"];

#pragma warning disable EXTEXP0018
        services.AddHybridCache();
#pragma warning restore EXTEXP0018

        services.AddScoped<IIdempotencyService, IdempotencyService>(sp =>
            new IdempotencyService(sp.GetRequiredService<HybridCache>()));

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Overseer")));

        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddScoped<IVersioningContainerRepository, VersioningContainerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IContainerRepository, ContainerRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
