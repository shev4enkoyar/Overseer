using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Overseer.WebAPI;
using Overseer.WebAPI.Application;
using Overseer.WebAPI.Endpoints;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Data;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApplicationServices()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IApiErrorHandler, ApiErrorHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi("v1", options =>
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddAuthorization();

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer(
        "overseer-keycloak",
        "overseer",
        options =>
        {
            options.RequireHttpsMetadata = false;
            options.Audience = "account";
            options.MetadataAddress = "https+http://overseer-keycloak/realms/overseer/.well-known/openid-configuration";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https+http://overseer-keycloak/realms/overseer",
            };
        });

builder.Services.AddAuthorizationBuilder();

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithModels(false)
            .WithClientButton(false)
            .WithTitle("Overseer")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithHttpBearerAuthentication(bearerOptions => bearerOptions.Token = "your-bearer-token"));
}

app.MapGet("/authentication/me", (ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.Claims.ToDictionary(x => x.Type, x => x.Value))
    .RequireAuthorization();

string[] summaries =
    ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
    {
        WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
#pragma warning disable CA5394
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
#pragma warning restore CA5394
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .RequireAuthorization();

app.MapDefaultEndpoints();

app.MapEndpoints();

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();

#pragma warning disable S3903
internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
#pragma warning restore S3903
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
