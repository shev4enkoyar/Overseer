namespace Overseer.FluentExtensions.Result;

public interface IResult<T> : IResult
{
    T Value { get; set; }
}

public interface IResult
{
    bool IsSuccessful { get; }

    bool IsFailure { get; }

    Error.Error Error { get; }
}
