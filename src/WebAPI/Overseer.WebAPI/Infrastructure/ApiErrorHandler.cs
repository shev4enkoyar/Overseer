using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Overseer.WebAPI.Application.Common.Exceptions;

namespace Overseer.WebAPI.Infrastructure;

internal sealed class ApiErrorHandler : IApiErrorHandler
{
    public IResult Handle(Error error)
    {
        if (error.Exception.IsSome)
        {
            Exception? ex = error.Exception.Match(
                static e => e,
                static () => null!
            );
            switch (ex)
            {
                case ValidationException vex:
                    {
                        var details = new ValidationProblemDetails(vex.Errors)
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                        };
                        return Results.Json(details, statusCode: StatusCodes.Status400BadRequest);
                    }
                case NotFoundException nfex:
                    {
                        var details = new ProblemDetails
                        {
                            Status = StatusCodes.Status404NotFound,
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                            Title = "The specified resource was not found.",
                            Detail = nfex.Message
                        };
                        return Results.NotFound(details);
                    }
                case UnauthorizedAccessException:
                    {
                        var details = new ProblemDetails
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Unauthorized",
                            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                        };
                        return Results.Json(details, statusCode: StatusCodes.Status401Unauthorized);
                    }
                case ForbiddenAccessException:
                    {
                        var details = new ProblemDetails
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Forbidden",
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                        };
                        return Results.Json(details, statusCode: StatusCodes.Status403Forbidden);
                    }
                default:
                    {
                        var details = new ProblemDetails
                        {
                            Status = StatusCodes.Status500InternalServerError,
                            Title = "Unhandled exception",
                            Detail = ex.Message
                        };
                        return Results.Problem(
                            title: details.Title,
                            detail: details.Detail,
                            statusCode: details.Status
                        );
                    }
            }
        }

        string detail = !string.IsNullOrWhiteSpace(error.Message)
            ? error.Message
            : "Unknown error";

        var otherDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Request error",
            Detail = detail
        };
        return Results.Json(otherDetails, statusCode: StatusCodes.Status400BadRequest);
    }
}
