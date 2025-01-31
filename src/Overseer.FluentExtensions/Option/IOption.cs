namespace Overseer.FluentExtensions.Option;

public interface IOption<out T>
{
    public bool IsSome { get; }

    public bool IsNone => !IsSome;

    public T? Value { get; }
}
