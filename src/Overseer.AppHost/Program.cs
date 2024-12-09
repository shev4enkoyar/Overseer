var builder = DistributedApplication.CreateBuilder(args);

var databaseUsername = builder.AddParameter("DatabaseUsername", secret: true);
var databasePassword = builder.AddParameter("DatabasePassword", secret: true);

var database = builder.AddPostgres("overseer-postgresql", databaseUsername, 
        databasePassword, port: 5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("overseer-postgresql-main-database");

var cache = builder.AddRedis("overseer-redis-cache")
    .WithLifetime(ContainerLifetime.Persistent);

var apiService = builder.AddProject<Projects.Overseer_ApiService>("web-api");

builder.AddProject<Projects.Overseer_Web>("frontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
