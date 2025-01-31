using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Overseer.FluentExtensions.Error;
using ValidationException = Overseer.WebAPI.Application.Common.Exceptions.ValidationException;

namespace Overseer.WebAPI.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var error = Error.Create(new ValidationException(failures));
        if (!typeof(TResponse).IsGenericType || typeof(TResponse).GetGenericTypeDefinition() != typeof(Result<>))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        Type resultType = typeof(TResponse).GetGenericArguments()[0];
        Type constructedResultType = typeof(Result<>).MakeGenericType(resultType);
        object failureResult = Activator.CreateInstance(constructedResultType, error)!;

        return (TResponse)failureResult;
    }
}
