namespace Overseer.FluentExtensions.Result;

public readonly struct Result : IResult, IEquatable<Result>
{
    public Result() => IsSuccessful = true;

    public Result(Error.Error error)
    {
        Error = error;
        IsSuccessful = false;
    }

    public bool IsSuccessful { get; }

    public bool IsFailure => !IsSuccessful;

    public Error.Error Error { get; }

    public static Result Success() => new();
    public static Result Failure(Error.Error error) => new(error);

    public readonly bool Equals(Result other) =>
        IsSuccessful == other.IsSuccessful && Equals(Error, other.Error);

    public override readonly bool Equals(object? obj) => obj is Result other && Equals(other);

    public override readonly int GetHashCode() => HashCode.Combine(IsSuccessful, Error);

    public static bool operator ==(Result left, Result right) => left.Equals(right);

    public static bool operator !=(Result left, Result right) => !left.Equals(right);

    public static implicit operator Result(Error.Error error) => Failure(error);
}

public struct Result<T> : IResult<T>, IEquatable<Result<T>>
{
    public Result(T value)
    {
        Value = value;
        IsSuccessful = true;
    }

    public Result(Error.Error error)
    {
        Error = error;
        Value = default!;
        IsSuccessful = false;
    }

    public bool IsSuccessful { get; }

    public readonly bool IsFailure => !IsSuccessful;

    public Error.Error Error { get; }

    public T Value { get; set; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error.Error error) => new(error);

    public readonly bool Equals(Result<T> other) => IsSuccessful == other.IsSuccessful &&
                                                    Equals(Error, other.Error) &&
                                                    EqualityComparer<T?>.Default.Equals(Value, other.Value);

    public override readonly bool Equals(object? obj) => obj is Result<T> other && Equals(other);

    public override readonly int GetHashCode() => HashCode.Combine(IsSuccessful, Error, Value);

    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    public static bool operator !=(Result<T> left, Result<T> right) => !left.Equals(right);

    public static implicit operator Result<T>(T value) => Success(value);

    public static explicit operator T(Result<T> result) => result.Value ?? throw new InvalidOperationException();

    public static implicit operator Result<T>(Error.Error error) => Failure(error);
}
