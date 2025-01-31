using System.Diagnostics.CodeAnalysis;

namespace Overseer.FluentExtensions.Option;

public readonly struct Option<T> : IOption<T>, IEquatable<Option<T>>
{
    private Option(T? value)
    {
        Value = value;
        if (Equals(value, default(T)))
        {
            IsSome = false;
            return;
        }

        IsSome = true;
    }

    public bool IsSome { get; }

    public bool IsNone => !IsSome;

    [MemberNotNullWhen(true, nameof(IsSome))]
    public T? Value { get; }

    public bool Equals(Option<T> other) =>
        IsSome == other.IsSome && EqualityComparer<T>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(IsSome, Value);

    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);

    public static implicit operator Option<T>(T? value) => new(value);
}
