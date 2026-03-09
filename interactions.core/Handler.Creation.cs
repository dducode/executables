using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Core.Internal;
using Interactions.Core.Providers;
using Interactions.Core.Resolvers;

namespace Interactions.Core;

public static class Handler {

  /// <summary>
  /// Creates a handler that resolves its inner handler lazily on first use.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="resolver">Resolver for the inner handler.</param>
  [Pure]
  public static Handler<T1, T2> Lazy<T1, T2>(IResolver<Handler<T1, T2>> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new LazyHandler<T1, T2>(resolver);
  }

  /// <summary>
  /// Creates a handler that resolves a new inner handler on every call.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="provider">Provider for per-call handler instances.</param>
  [Pure]
  public static Handler<T1, T2> Dynamic<T1, T2>(IProvider<Handler<T1, T2>> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new DynamicHandler<T1, T2>(provider);
  }

}