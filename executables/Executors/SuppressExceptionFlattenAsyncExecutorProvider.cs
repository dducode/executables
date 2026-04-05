using Executables.Core.Executors;

namespace Executables.Executors;

/// <summary>
/// Provides suppression executor for asynchronous executors that already return <see cref="Optional{T}" />.
/// </summary>
public readonly ref struct SuppressExceptionFlattenAsyncExecutorProvider<T1, T2>(IAsyncExecutor<T1, Optional<T2>> executor) {

  /// <summary>
  /// Suppresses exceptions of the specified type and preserves optional result shape.
  /// </summary>
  /// <typeparam name="TEx">Exception type to suppress.</typeparam>
  /// <returns>Async executor that returns an empty optional when suppressed exception occurs.</returns>
  public IAsyncExecutor<T1, Optional<T2>> OfType<TEx>() where TEx : Exception {
    return new SuppressExceptionFlattenAsyncExecutor<T1, T2, TEx>(executor);
  }

}