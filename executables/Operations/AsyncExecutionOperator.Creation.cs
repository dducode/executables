using System.Diagnostics.Contracts;
using Executables.Analytics;
using Executables.Core.Operators;
using Executables.Internal;

namespace Executables.Operations;

/// <summary>
/// Factory methods for creating asynchronous execution operators.
/// </summary>
public static class AsyncExecutionOperator {

  /// <summary>
  /// Creates an asynchronous cache operator that reuses results stored in the provided cache storage.
  /// </summary>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Cache operator.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="storage"/> is <see langword="null"/>.</exception>
  [Pure]
  public static AsyncBehaviorOperator<T1, T2> Cache<T1, T2>(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new AsyncCacheOperator<T1, T2>(storage);
  }

  /// <summary>
  /// Creates an asynchronous metrics operator that records call, success, failure and latency for the wrapped executor.
  /// </summary>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag passed to all <see cref="IMetrics{T1,T2}"/> callbacks.</param>
  /// <returns>Metrics operator.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metrics"/> is <see langword="null"/>.</exception>
  [Pure]
  public static AsyncBehaviorOperator<T1, T2> Metrics<T1, T2>(IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new AsyncMetricsOperator<T1, T2>(metrics, tag);
  }

  /// <summary>
  /// Creates an asynchronous operator that maps input before execution and output after execution.
  /// </summary>
  /// <param name="incoming">Executable that converts external input to the wrapped executor input type.</param>
  /// <param name="outgoing">Executable that converts wrapped executor output to the final output type.</param>
  /// <returns>Asynchronous execution operator that applies both mappings around the wrapped executor.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> or <paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static AsyncExecutionOperator<T1, T2, T3, T4> Map<T1, T2, T3, T4>(IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing) {
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new AsyncMap<T1, T2, T3, T4>(incoming, outgoing);
  }

  /// <summary>
  /// Creates an asynchronous execution operator from a delegate.
  /// </summary>
  /// <param name="operation">Delegate implementing operator behavior.</param>
  /// <returns>Execution operator wrapping <paramref name="operation"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
  [Pure]
  public static AsyncExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(AsyncFunc<T1, IAsyncExecutor<T2, T3>, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AsyncAnonymousOperator<T1, T2, T3, T4>(operation);
  }

}
