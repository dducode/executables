using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

/// <summary>
/// Factory methods for creating <see cref="Handler{T1,T2}"/>.
/// <example>
/// <code>
/// var handler = Handler.Create((int i) => i + 1);
/// var result = handler.Execute(41); // 42
/// </code>
/// </example>
/// </summary>
public static class Handler {

  /// <summary>
  /// Returns a handler that returns its input unchanged.
  /// </summary>
  /// <typeparam name="T">Input and output type.</typeparam>
  [Pure]
  public static Handler<T, T> Identity<T>() {
    return new IdentityHandler<T>();
  }

  [Pure]
  public static Handler<Unit, Unit> Identity() {
    return Identity<Unit>();
  }

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

  /// <summary>
  /// Creates a handler from a function.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="func">Function used for handling.</param>
  [Pure]
  public static Handler<T1, T2> Create<T1, T2>(Func<T1, T2> func) {
    return Executable.Create(func).AsHandler();
  }

  /// <summary>
  /// Creates a handler from a parameterless function.
  /// </summary>
  /// <typeparam name="T">Output type.</typeparam>
  /// <param name="func">Function used for handling.</param>
  [Pure]
  public static Handler<Unit, T> Create<T>(Func<T> func) {
    return Executable.Create(func).AsHandler();
  }

  /// <summary>
  /// Creates a handler from an action.
  /// </summary>
  /// <typeparam name="T">Input type.</typeparam>
  /// <param name="action">Action used for handling.</param>
  [Pure]
  public static Handler<T, Unit> Create<T>(Action<T> action) {
    return Executable.Create(action).AsHandler();
  }

  /// <summary>
  /// Creates a handler from a parameterless action.
  /// </summary>
  /// <param name="action">Action used for handling.</param>
  [Pure]
  public static Handler<Unit, Unit> Create(Action action) {
    return Executable.Create(action).AsHandler();
  }

}