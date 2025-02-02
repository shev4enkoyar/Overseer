using System.Security.Claims;
using Overseer.WebAPI;
using Overseer.WebAPI.Application;
using Overseer.WebAPI.Endpoints;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Data;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApplicationServices()
    .AddWebServices(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.AddRedisDistributedCache("overseer-redis-cache");

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    app.MapOpenApi();
    app.MapScalarApiReference(static options =>
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithModels(false)
            .WithClientButton(false)
            .WithTitle("Overseer")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithHttpBearerAuthentication(static bearerOptions => bearerOptions.Token = string.Empty));
}

app.MapGet("/authentication/me", static (ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.Claims.Select(static x => $"{x.Type} : {x.Value}"))
    .RequireAuthorization();

app.MapDefaultEndpoints();

app.MapEndpoints();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

await app.RunAsync();
