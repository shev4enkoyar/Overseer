namespace Overseer.WebAPI.Infrastructure.Configuration;

internal sealed class RateLimiterSettings
{
    internal const string TokenPolicy = "token";

    internal const string ConfigurationSectionName = "RateLimiter";

    public int TokenLimit { get; set; }
    public int TokensPerPeriod { get; set; }
    public int ReplenishmentPeriodSeconds { get; set; }
}
