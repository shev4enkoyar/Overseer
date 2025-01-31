namespace Overseer.FluentExtensions.Option;

public static class IOptionExtensions
{
    public static TMatch Map<T, TMatch>(this IOption<T> option, Func<T, TMatch> onSome, Func<TMatch> onNone) =>
        option.IsSome ? onSome(option.Value!) : onNone();

    public static Task<TMatch> MapAsync<T, TMatch>(this IOption<T> option, Func<T, Task<TMatch>> onSome,
        Func<Task<TMatch>> onNone) => option.IsSome ? onSome(option.Value!) : onNone();

    public static Task<TMatch> MapAsync<T, TMatch>(this IOption<T> option, Func<T, Task<TMatch>> onSome,
        Func<TMatch> onNone) => option.IsSome ? onSome(option.Value!) : Task.FromResult(onNone());
}
