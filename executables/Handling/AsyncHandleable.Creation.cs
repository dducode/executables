using System.Diagnostics.Contracts;
using Executables.Core.Handleables;
using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Factory methods for creating asynchronous handleables.
/// </summary>
public static class AsyncHandleable {

  /// <summary>
  /// Creates an asynchronous handleable from a registration delegate.
  /// </summary>
  /// <param name="handling">Delegate that registers a handler and returns a handle for unregistering it.</param>
  /// <returns>Asynchronous handleable backed by <paramref name="handling"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="handling"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncHandleable<T1, T2> Create<T1, T2>(Func<AsyncHandler<T1, T2>, IDisposable> handling) {
    ExceptionsHelper.ThrowIfNull(handling, nameof(handling));
    return new AsyncAnonymousHandleable<T1, T2>(handling);
  }

}