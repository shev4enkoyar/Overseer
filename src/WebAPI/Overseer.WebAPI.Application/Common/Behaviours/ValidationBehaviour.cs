using System.Reflection;
using FluentValidation;
using MediatR;
using ValidationException = Overseer.WebAPI.Application.Common.Exceptions.ValidationException;

namespace Overseer.WebAPI.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IEither
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) 
            return await next();
        
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count != 0)
            return CreateDynamicEither(Error.New(new ValidationException(failures)));
        return await next();
    }
    
    private TResponse CreateDynamicEither(Error error)
    {
        var rightType = typeof(TResponse).GetGenericArguments()[1];
        var eitherType = typeof(Either<,>).MakeGenericType(typeof(Error), rightType);
        var leftMethod = eitherType.GetMethod("Left", BindingFlags.Static | BindingFlags.Public);
        if (leftMethod == null)
        {
            throw new InvalidOperationException("Method 'Left' not found on Either.");
        }
        var eitherInstance = leftMethod.Invoke(null, [error]);
        return (TResponse)eitherInstance!;
    }
}