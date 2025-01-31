namespace Overseer.FluentExtensions.Error;

public interface IError
{
    string Message { get; }
    Exception? Exception { get; }
}
