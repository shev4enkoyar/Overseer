using LanguageExt.Common;

namespace Overseer.WebAPI.Infrastructure;

internal interface IApiErrorHandler
{
    IResult Handle(Error error);
}
