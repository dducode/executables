using System.Diagnostics.Contracts;
using Executables.Core.Handlers;
using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Extension methods for asynchronous handlers.
/// </summary>
public static class AsyncHandlersExtensions {

  /// <summary>
  /// Adds disposal callback behavior to a handler.
  /// </summary>
  /// <param name="handler">Source handler.</param>
  /// <param name="dispose">Callback invoked during disposal.</param>
  /// <returns>Handler with disposal callback behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <see langword="null"/>.</exception>
  [Pure]
  public static AsyncHandler<T1, T2> OnDispose<T1, T2>(this AsyncHandler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AsyncAnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

  /// <summary>
  /// Disposes the handler automatically when an unhandled exception escapes execution.
  /// </summary>
  /// <param name="handler">Source handler.</param>
  /// <returns>Auto-disposing handler wrapper.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> DisposeOnUnhandledException<T1, T2>(this AsyncHandler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AsyncAutoDisposedHandler<T1, T2>(handler);
  }

}