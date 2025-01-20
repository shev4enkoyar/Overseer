namespace Overseer.WebAPI.Infrastructure;

public interface IEndpointGroup
{
    public static abstract RouteGroupBuilder MapGroup(WebApplication app);
}