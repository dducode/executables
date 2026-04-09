using Executables.Core.Executors;

namespace Executables.Executors;

/// <summary>
/// Provides suppression executor for executors that already return <see cref="Optional{T}" />.
/// </summary>
public readonly ref struct SuppressExceptionFlattenExecutorProvider<T1, T2>(IExecutor<T1, Optional<T2>> executor) {

  /// <summary>
  /// Suppresses exceptions of the specified type and preserves optional result shape.
  /// </summary>
  /// <typeparam name="TEx">Exception type to suppress.</typeparam>
  /// <returns>Executor that returns an empty optional when suppressed exception occurs.</returns>
  public IExecutor<T1, Optional<T2>> OfType<TEx>() where TEx : Exception {
    return new SuppressExceptionFlattenExecutor<T1, T2, TEx>(executor);
  }

}
