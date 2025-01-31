using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Overseer.FluentExtensions.Error;

public readonly partial struct Error : IError
{
    [JsonPropertyName("message")] public string Message { get; }

    [JsonPropertyName("exception")] public Exception? Exception { get; }

    [JsonPropertyName("exception-type")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public string? ExceptionType { get; } = null;

    private Error(string message) => Message = message;

    private Error(Exception exception)
    {
        Exception = exception;
        Message = exception.Message;
        ExceptionType = exception.GetType().AssemblyQualifiedName;
    }

    private Error(string message, Exception exception)
    {
        Message = message;
        Exception = exception;
        ExceptionType = exception.GetType().AssemblyQualifiedName;
    }

    [JsonConstructor]
    public Error(string message, Exception exception, string? exceptionType)
    {
        Message = message;
        Exception = exception;
        ExceptionType = exceptionType;
        if (exceptionType == null)
        {
            return;
        }

        var type = Type.GetType(exceptionType);
        Exception = (Exception)Activator.CreateInstance(type!, exception.Message, exception.Source)!;
    }

    public override string ToString() => Message;
}
