using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Operations;
using Interactions.Policies;

namespace Interactions;

/// <summary>
/// Factory methods for creating synchronous execution operators.
/// </summary>
public static class ExecutionOperator {

  /// <summary>
  /// Returns an operator that simply forwards execution to the wrapped executor.
  /// </summary>
  /// <returns>Identity operator.</returns>
  [Pure]
  public static ExecutionOperator<T1, T1, T2, T2> Identity<T1, T2>() {
    return IdentityOperator<T1, T2>.Instance;
  }

  /// <summary>
  /// Creates a cache operator that reuses results stored in the provided cache storage.
  /// </summary>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Cache operator.</returns>
  [Pure]
  public static BehaviorOperator<T1, T2> Cache<T1, T2>(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new CacheOperator<T1, T2>(storage);
  }

  /// <summary>
  /// Creates a cache operator for the same input and output type.
  /// </summary>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Cache operator.</returns>
  [Pure]
  public static BehaviorOperator<T, T> Cache<T>(ICacheStorage<T, T> storage) {
    return Cache<T, T>(storage);
  }

  /// <summary>
  /// Creates a metrics operator that records execution metrics for the wrapped executor.
  /// </summary>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag associated with recorded metrics.</param>
  /// <returns>Metrics operator.</returns>
  [Pure]
  public static BehaviorOperator<T1, T2> Metrics<T1, T2>(this IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new MetricsOperator<T1, T2>(metrics, tag);
  }

  /// <summary>
  /// Creates a metrics operator for the same input and output type.
  /// </summary>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag associated with recorded metrics.</param>
  /// <returns>Metrics operator.</returns>
  [Pure]
  public static BehaviorOperator<T, T> Metrics<T>(this IMetrics<T, T> metrics, string tag = null) {
    return Metrics<T, T>(metrics, tag);
  }

  /// <summary>
  /// Creates an operator that maps input before execution and output after execution.
  /// </summary>
  /// <param name="incoming">Executable that converts external input to the wrapped executor input type.</param>
  /// <param name="outgoing">Executable that converts wrapped executor output to the final output type.</param>
  /// <returns>Execution operator that applies both mappings around the wrapped executor.</returns>
  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Map<T1, T2, T3, T4>(IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing) {
    ExceptionsHelper.ThrowIfNull(incoming, nameof(incoming));
    ExceptionsHelper.ThrowIfNull(outgoing, nameof(outgoing));
    return new Map<T1, T2, T3, T4>(incoming, outgoing);
  }

  /// <summary>
  /// Creates an execution operator from a delegate.
  /// </summary>
  /// <param name="operation">Delegate implementing operator behavior.</param>
  /// <returns>Execution operator wrapping <paramref name="operation"/>.</returns>
  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Func<T1, IExecutor<T2, T3>, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AnonymousOperator<T1, T2, T3, T4>(operation);
  }

}