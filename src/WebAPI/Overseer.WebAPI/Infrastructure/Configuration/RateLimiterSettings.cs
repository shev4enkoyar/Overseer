namespace Overseer.WebAPI.Infrastructure.Configuration;

internal sealed class RateLimiterSettings
{
    internal const string TokenPolicy = "token";

    internal const string ConfigurationSectionName = "RateLimiter";

    internal int TokenLimit { get; set; }
    internal int TokensPerPeriod { get; set; }
    internal int ReplenishmentPeriodSeconds { get; set; }
}
