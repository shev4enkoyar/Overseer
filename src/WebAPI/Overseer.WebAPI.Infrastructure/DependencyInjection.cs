using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Overseer.WebAPI.Application.Common.Interfaces;
using Overseer.WebAPI.Infrastructure.Data;
using Overseer.WebAPI.Infrastructure.Data.Repositories;

namespace Overseer.WebAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? databaseConnectionString = configuration["ConnectionStings:Database"];

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Overseer")));

        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddScoped<IVersioningContainerRepository, VersioningContainerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
