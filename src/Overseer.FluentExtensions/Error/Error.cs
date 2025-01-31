using System.Text.Json.Serialization;

namespace Overseer.FluentExtensions.Error;

public readonly struct Error : IError, IEquatable<Error>
{
    [JsonPropertyName("message")] public string Message { get; }

    [JsonPropertyName("exception")] public Exception? Exception { get; }

    [JsonPropertyName("exception-type")] public string? ExceptionType { get; } = null;

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
        if (exceptionType != null)
        {
            var type = Type.GetType(exceptionType);
            Exception = (Exception)Activator.CreateInstance(type!, exception.Message, exception.Source)!;
        }
    }

    public static Error Create(string message) => new(message);

    public static Error Create(Exception exception) => new(exception);

    public static Error Create(string message, Exception exception) => new(message, exception);

    public override string ToString() => Message;

    public bool Equals(Error other) => Message == other.Message && Equals(Exception, other.Exception);

    public override bool Equals(object? obj) => obj is Error other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Message, Exception);

    public static bool operator ==(Error left, Error right) => left.Equals(right);

    public static bool operator !=(Error left, Error right) => !left.Equals(right);

    public static implicit operator Error(Exception exception) => new(exception);
}
