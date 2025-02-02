namespace Overseer.WebAPI.Extensions;

internal static class EndpointExtensions
{
    internal static TBuilder WithRateLimiting<TBuilder>(this TBuilder builder, string policyName)
        where TBuilder : IEndpointConventionBuilder => builder
        .ProducesProblem(StatusCodes.Status429TooManyRequests)
        .RequireRateLimiting(policyName);
}
