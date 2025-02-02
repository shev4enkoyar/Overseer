using System.Threading.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Configuration;

namespace Overseer.WebAPI;

internal static class DependencyInjection
{
    internal static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApiErrorHandler, ApiErrorHandler>();

        services.AddProblemDetails();

        services.AddOpenApi("v1", static options =>
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        services.AddAuthorization();

        services.AddAuthentication()
            .AddKeycloakJwtBearer(
                "overseer-keycloak",
                "overseer",
                static options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";
                    options.MetadataAddress =
                        "https+http://overseer-keycloak/realms/overseer/.well-known/openid-configuration";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "https+http://overseer-keycloak/realms/overseer"
                    };
                });

        services.AddAuthorizationBuilder();

        services.AddConfiguredRateLimiters(configuration);

        return services;
    }

    private static void AddConfiguredRateLimiters(this IServiceCollection services, IConfiguration configuration)
    {
        RateLimiterSettings rateLimiterSettings =
            configuration.GetSection(RateLimiterSettings.ConfigurationSectionName)
                .Get<RateLimiterSettings>()
            ?? throw new ArgumentException("Rate limiter settings not found in configuration.", nameof(configuration));

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(RateLimiterSettings.TokenPolicy, context =>
                RateLimitPartition.GetTokenBucketLimiter(
                    $"{context.Connection.RemoteIpAddress};{context.Request.Path.Value?.ToUpperInvariant()}",
                    _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = rateLimiterSettings.TokenLimit,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimiterSettings.ReplenishmentPeriodSeconds),
                        TokensPerPeriod = rateLimiterSettings.TokensPerPeriod
                    }));
        });
    }
}
