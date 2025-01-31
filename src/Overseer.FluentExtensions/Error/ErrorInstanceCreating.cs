namespace Overseer.FluentExtensions.Error;

public partial struct Error
{
    public static Error Create(string message) => new(message);

    public static Error Create(Exception exception) => new(exception);

    public static Error Create(string message, Exception exception) => new(message, exception);

    public static implicit operator Error(Exception exception) => new(exception);
}
