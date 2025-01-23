using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = Overseer.WebAPI.Application.Common.Exceptions.ValidationException;

namespace Overseer.WebAPI.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IEither
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

        if (failures.Count != 0)
        {
            return CreateDynamicEither(Error.New(new ValidationException(failures)));
        }

        return await next();
    }

    private static TResponse CreateDynamicEither(Error error)
    {
        Type rightType = typeof(TResponse).GetGenericArguments()[1];
        Type eitherType = typeof(Either<,>).MakeGenericType(typeof(Error), rightType);
        MethodInfo leftMethod = eitherType.GetMethod("Left", BindingFlags.Static | BindingFlags.Public)
                                ?? throw new InvalidOperationException("Method 'Left' not found on Either.");

        object? eitherInstance = leftMethod.Invoke(null, [error]);
        return (TResponse)eitherInstance!;
    }
}
