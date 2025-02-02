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

builder.AddRedisDistributedCache("overseer-redis-cache");

builder.Services.AddScoped<IApiErrorHandler, ApiErrorHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi("v1", static options =>
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddAuthorization();

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer(
        "overseer-keycloak",
        "overseer",
        static options =>
        {
            options.RequireHttpsMetadata = false;
            options.Audience = "account";
            options.MetadataAddress = "https+http://overseer-keycloak/realms/overseer/.well-known/openid-configuration";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https+http://overseer-keycloak/realms/overseer"
            };
        });

builder.Services.AddAuthorizationBuilder();

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

await app.RunAsync();
