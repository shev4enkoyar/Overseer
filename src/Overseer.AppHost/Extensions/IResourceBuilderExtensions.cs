// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable InvalidXmlDocComment
namespace Overseer.AppHost.Extensions;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Provides extension methods for working with <see cref="IResourceBuilder"/>.
/// </summary>
internal static class IResourceBuilderExtensions
{
    /// <summary>
    /// Adds the <paramref name="source"/> resource as a dependency to the current resource <paramref name="builder"/>,
    /// and configures waiting for the readiness of this dependent resource.
    /// </summary>
    /// <typeparam name="TDestination">
    /// The type of the target resource, which must implement 
    /// <see cref="IResourceWithEnvironment"/> and <see cref="IResourceWithWaitSupport"/>.
    /// </typeparam>
    /// <param name="builder">The resource builder to which the dependency is added.</param>
    /// <param name="source">The builder of the dependent resource that provides a ConnectionString.</param>
    /// <returns>
    /// A builder of the resource <typeparamref name="TDestination"/>, configured to wait for the readiness of the dependent resource.
    /// </returns>
    internal static IResourceBuilder<TDestination> WithWaitingReference<TDestination>(
        this IResourceBuilder<TDestination> builder, 
        IResourceBuilder<IResourceWithConnectionString> source) 
        where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
    {
        return builder.WithReference(source)
            .WaitFor(source);
    }
    
    /// <summary>
    /// Adds the <paramref name="source"/> resource as a dependency to the current resource <paramref name="builder"/>,
    /// and configures waiting for the readiness of this dependent resource.
    /// </summary>
    /// <typeparam name="TDestination">
    /// The type of the target resource, which must implement 
    /// <see cref="IResourceWithEnvironment"/> and <see cref="IResourceWithWaitSupport"/>.
    /// </typeparam>
    /// <param name="builder">The resource builder to which the dependency is added.</param>
    /// <param name="source">The builder of the dependent resource that provides ServiceDiscovery capabilities.</param>
    /// <returns>
    /// A builder of the resource <typeparamref name="TDestination"/>, configured to wait for the readiness of the dependent resource.
    /// </returns>
    internal static IResourceBuilder<TDestination> WithWaitingReference<TDestination>(
        this IResourceBuilder<TDestination> builder, 
        IResourceBuilder<IResourceWithServiceDiscovery> source) 
        where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
    {
        return builder.WithReference(source)
            .WaitFor(source);
    }
}