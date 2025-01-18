var builder = DistributedApplication.CreateBuilder(args);

var databaseUsername = builder.AddParameter("DatabaseUsername", secret: true);
var databasePassword = builder.AddParameter("DatabasePassword", secret: true);

var database = builder.AddPostgres("overseer-postgresql", databaseUsername, 
        databasePassword, port: 5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("overseer-postgresql-database");

var cache = builder.AddRedis("overseer-redis-cache")
    .WithLifetime(ContainerLifetime.Persistent);

var apiService = builder.AddProject<Projects.Overseer_WebAPI>("overseer-web-api")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.Overseer_WebUI>("overseer-web-ui")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
