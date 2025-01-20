namespace Overseer.WebAPI.Infrastructure;

internal interface IEndpoint
{
    public static abstract void MapEndpoint(RouteGroupBuilder routeGroupBuilder);
}