using System.Diagnostics.Contracts;
using Executables.Core.Handleables;
using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Extension methods for asynchronous handleable objects.
/// </summary>
public static class AsyncHandleableExtensions {

  /// <summary>
  /// Merges two asynchronous handleables into one registration target.
  /// </summary>
  /// <param name="first">First handleable.</param>
  /// <param name="second">Second handleable.</param>
  /// <returns>Composite handleable that registers handlers in both sources.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncHandleable<T1, T2, THandler> Merge<T1, T2, THandler>(
    this IAsyncHandleable<T1, T2, THandler> first,
    IAsyncHandleable<T1, T2, THandler> second) where THandler : AsyncHandler<T1, T2> {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeHandleable<T1, T2, THandler>(first, second);
  }

  /// <summary>
  /// Registers an asynchronous function as a handler.
  /// </summary>
  public static IDisposable Handle<T1, T2>(this IAsyncHandleable<T1, T2> handleable, AsyncFunc<T1, T2> handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  /// <summary>
  /// Registers a parameterless asynchronous function as a handler.
  /// </summary>
  public static IDisposable Handle<T>(this IAsyncHandleable<Unit, T> handleable, AsyncFunc<T> handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  /// <summary>
  /// Registers an asynchronous action as a handler.
  /// </summary>
  public static IDisposable Handle<T>(this IAsyncHandleable<T, Unit> handleable, AsyncAction<T> handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  /// <summary>
  /// Registers a parameterless asynchronous action as a handler.
  /// </summary>
  public static IDisposable Handle(this IAsyncHandleable<Unit, Unit> handleable, AsyncAction handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

}