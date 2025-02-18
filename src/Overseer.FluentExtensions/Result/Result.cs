﻿using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Overseer.FluentExtensions.Result;

public readonly struct Result : IResult, IEquatable<Result>
{
    [JsonConstructor]
    public Result(bool isSuccessful, Error.Error error)
    {
        IsSuccessful = isSuccessful;
        Error = error;
    }

    public Result() => IsSuccessful = true;

    public Result(Error.Error error)
    {
        Error = error;
        IsSuccessful = false;
    }

    [JsonPropertyName("isSuccessful")] public bool IsSuccessful { get; }

    [JsonIgnore] public bool IsFailure => !IsSuccessful;

    [JsonPropertyName("error")] public Error.Error Error { get; }

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
    [JsonConstructor]
    public Result(bool isSuccessful, T value, Error.Error error)
    {
        IsSuccessful = isSuccessful;
        Value = value;
        Error = error;
    }

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

    [JsonPropertyName("isSuccessful")]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccessful { get; }

    [JsonIgnore]
    [MemberNotNullWhen(false, nameof(Value))]
    public readonly bool IsFailure => !IsSuccessful;

    [JsonPropertyName("error")] public Error.Error Error { get; }

    [JsonPropertyName("value")] public T Value { get; set; }

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
