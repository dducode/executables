using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Core.Internal;
using Interactions.Core.Providers;
using Interactions.Core.Resolvers;

namespace Interactions.Core;

public static class AsyncHandler {

  /// <summary>
  /// Creates an async handler that resolves its inner handler lazily on first use.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="resolver">Resolver for the inner async handler.</param>
  [Pure]
  public static AsyncHandler<T1, T2> Lazy<T1, T2>(IResolver<AsyncHandler<T1, T2>> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new AsyncLazyHandler<T1, T2>(resolver);
  }

  /// <summary>
  /// Creates an async handler that resolves a new inner handler on every call.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="provider">Provider for per-call async handler instances.</param>
  [Pure]
  public static AsyncHandler<T1, T2> Dynamic<T1, T2>(IProvider<AsyncHandler<T1, T2>> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new AsyncDynamicHandler<T1, T2>(provider);
  }

}