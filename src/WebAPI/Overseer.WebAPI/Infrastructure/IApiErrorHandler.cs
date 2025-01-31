using Overseer.FluentExtensions.Error;

namespace Overseer.WebAPI.Infrastructure;

internal interface IApiErrorHandler
{
    IResult Handle(Error error);
}
