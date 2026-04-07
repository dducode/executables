using System.Diagnostics.Contracts;
using Executables.Core.Handlers;
using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Extension methods for handlers.
/// </summary>
public static class HandlersExtensions {

  /// <summary>
  /// Converts a synchronous handler into an asynchronous handler.
  /// </summary>
  /// <param name="handler">Source handler.</param>
  /// <returns>Asynchronous proxy handler.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> ToAsyncHandler<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AsyncProxyHandler<T1, T2>(handler);
  }

  /// <summary>
  /// Adds disposal callback behavior to a handler.
  /// </summary>
  /// <param name="handler">Source handler.</param>
  /// <param name="dispose">Callback invoked during disposal.</param>
  /// <returns>Handler with disposal callback behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <see langword="null"/>.</exception>
  [Pure]
  public static Handler<T1, T2> OnDispose<T1, T2>(this Handler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

  /// <summary>
  /// Disposes the handler automatically when an unhandled exception escapes execution.
  /// </summary>
  /// <param name="handler">Source handler.</param>
  /// <returns>Auto-disposing handler wrapper.</returns>
  [Pure]
  public static Handler<T1, T2> DisposeOnUnhandledException<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AutoDisposedHandler<T1, T2>(handler);
  }

}