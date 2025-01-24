using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace Overseer.WebAPI.Application.Common.Exceptions;

[SuppressMessage("Design", "CA1032:Implement standard exception constructors")]
public class ValidationException() : Exception("One or more validation failures have occurred.")
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this() =>
        Errors = failures
            .GroupBy(static e => e.PropertyName, static e => e.ErrorMessage)
            .ToDictionary(static failureGroup => failureGroup.Key,
                static failureGroup => failureGroup.ToArray());

    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
}
