namespace Overseer.WebAPI.Infrastructure;

internal interface IEndpointGroup
{
    public static abstract RouteGroupBuilder MapGroup(WebApplication app);
}
