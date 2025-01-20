using LanguageExt.Common;

namespace Overseer.WebAPI.Infrastructure;

public interface IApiErrorHandler
{
    IResult Handle(Error error);
}