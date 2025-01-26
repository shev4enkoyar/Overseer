using Overseer.AppHost.Extensions;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> databaseUsername = builder.AddParameter("DatabaseUsername", true);
IResourceBuilder<ParameterResource> databasePassword = builder.AddParameter("DatabasePassword", true);

IResourceBuilder<PostgresDatabaseResource> database = builder.AddPostgres("overseer-postgresql", databaseUsername,
        databasePassword, 5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("overseer-postgresql-database");

IResourceBuilder<ParameterResource> keycloakUsername = builder.AddParameter("KeycloakUsername");
IResourceBuilder<ParameterResource> keycloakPassword = builder.AddParameter("KeycloakPassword", true);

IResourceBuilder<KeycloakResource> keycloak = builder.AddKeycloak("overseer-keycloak", 8080,
        keycloakUsername, keycloakPassword)
    .WithExternalHttpEndpoints()
    .WithRealmImport(Path.Combine(Directory.GetCurrentDirectory(), "Resources"))
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<RedisResource> cache = builder.AddRedis("overseer-redis-cache")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<ProjectResource> apiService = builder.AddProject<Overseer_WebAPI>("overseer-web-api")
    .WithExternalHttpEndpoints()
    .WithWaitingReference(keycloak)
    .WithWaitingReference(database);

builder.AddProject<Overseer_WebUI>("overseer-web-ui")
    .WithExternalHttpEndpoints()
    .WithWaitingReference(cache)
    .WithWaitingReference(keycloak)
    .WithWaitingReference(apiService);

DistributedApplication application = builder.Build();
await application.RunAsync();
