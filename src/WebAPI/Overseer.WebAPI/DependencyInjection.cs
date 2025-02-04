using System.Diagnostics.CodeAnalysis;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Configuration;

namespace Overseer.WebAPI;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
internal static class DependencyInjection
{
    internal static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApiErrorHandler, ApiErrorHandler>();

        services.AddProblemDetails();

        services.AddOpenApi("v1", static options =>
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

        services.AddAuthorization();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(static options =>
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
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        // ReSharper disable once UnusedVariable
#pragma warning disable IDE0008
#pragma warning disable S1481
        var some = configuration.GetSection(RateLimiterSettings.ConfigurationSectionName);
#pragma warning restore S1481
#pragma warning restore IDE0008
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
