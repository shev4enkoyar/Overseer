namespace Overseer.FluentExtensions.Error;

public readonly struct Error : IError, IEquatable<Error>
{
    public string Message { get; }
    public Exception? Exception { get; }

    private Error(string message) => Message = message;

    private Error(Exception exception)
    {
        Exception = exception;
        Message = exception.Message;
    }

    private Error(string message, Exception exception)
    {
        Message = message;
        Exception = exception;
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
