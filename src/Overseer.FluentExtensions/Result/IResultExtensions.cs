namespace Overseer.FluentExtensions.Result;

public static class IResultExtensions
{
    public static IResult Bind(this IResult result, Func<Result> function) =>
        result.IsSuccessful ? function.Invoke() : result;

    public static IResult<T> Bind<T>(this IResult<T> result, Func<T, Result<T>> function) =>
        result.IsSuccessful ? function.Invoke(result.Value) : result;

    public static TMatch Map<TMatch>(this IResult result, Func<TMatch> onSuccess,
        Func<Error.Error, TMatch> onFailure) =>
        result.IsSuccessful ? onSuccess() : onFailure(result.Error);

    public static TMatch Map<T, TMatch>(this IResult<T> result, Func<T, TMatch> onSuccess,
        Func<Error.Error, TMatch> onFailure) =>
        result.IsSuccessful ? onSuccess(result.Value) : onFailure(result.Error);
}
