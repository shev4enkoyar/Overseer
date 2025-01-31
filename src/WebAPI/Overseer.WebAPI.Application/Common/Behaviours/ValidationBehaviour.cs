using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Overseer.FluentExtensions.Error;
using ValidationException = Overseer.WebAPI.Application.Common.Exceptions.ValidationException;

namespace Overseer.WebAPI.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
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
            return CreateFailureResult(new ValidationException(failures));
        }

        return await next();
    }

    private static TResponse CreateFailureResult(Error error)
    {
        Type responseType = typeof(TResponse).GetGenericArguments()[0];
        Type finType = typeof(Result<>).MakeGenericType(responseType);
        MethodInfo[] failMethods = finType.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(static m => m.Name == "Failure" && m.GetParameters().Length == 1)
            .ToArray();

        MethodInfo finFailMethod = failMethods
                                       .FirstOrDefault(static m =>
                                           m.GetParameters()[0].ParameterType == typeof(Error)) ??
                                   throw new InvalidOperationException("Method 'Failure' not found on Result<T>.");

        object? result = finFailMethod.Invoke(null, [error]);
        return (TResponse)result!;
    }
}
