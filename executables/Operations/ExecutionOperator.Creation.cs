using System.Diagnostics.Contracts;
using Executables.Analytics;
using Executables.Context;
using Executables.Core.Operators;
using Executables.Internal;

namespace Executables.Operations;

/// <summary>
/// Factory methods for creating synchronous execution operators.
/// </summary>
public static class ExecutionOperator {

  /// <summary>
  /// Creates a cache operator that reuses results stored in the provided cache storage.
  /// </summary>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Cache operator.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="storage"/> is <see langword="null"/>.</exception>
  [Pure]
  public static BehaviorOperator<T1, T2> Cache<T1, T2>(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new CacheOperator<T1, T2>(storage);
  }

  /// <summary>
  /// Creates a metrics operator that records call, success, failure and latency for the wrapped executor.
  /// </summary>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag passed to all <see cref="IMetrics{T1,T2}"/> callbacks.</param>
  /// <returns>Metrics operator.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metrics"/> is <see langword="null"/>.</exception>
  [Pure]
  public static BehaviorOperator<T1, T2> Metrics<T1, T2>(IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new MetricsOperator<T1, T2>(metrics, tag);
  }

  /// <summary>
  /// Creates a context operator that runs execution inside a newly initialized context.
  /// </summary>
  /// <param name="init">Context initialization logic.</param>
  /// <returns>Context execution operator.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="init"/> is <see langword="null"/>.</exception>
  [Pure]
  public static BehaviorOperator<T1, T2> Context<T1, T2>(ContextInit init) {
    ExceptionsHelper.ThrowIfNull(init, nameof(init));
    return new ContextOperator<T1, T2>(init);
  }

  /// <summary>
  /// Creates an operator that maps exceptions of a specific type.
  /// </summary>
  /// <param name="map">Function that maps the caught exception to a new exception.</param>
  /// <returns>Exception-mapping operator.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static BehaviorOperator<T1, T2> MapException<T1, T2, TFrom>(Func<TFrom, Exception> map) where TFrom : Exception {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return new ExceptionMap<T1, T2, TFrom>(map);
  }

  /// <summary>
  /// Creates an operator that throttles repeated executions within the specified interval.
  /// </summary>
  /// <param name="interval">Minimum interval between forwarded executions.</param>
  /// <returns>Throttle operator.</returns>
  /// <exception cref="ArgumentException"><paramref name="interval"/> is less than or equal to zero.</exception>
  [Pure]
  public static BehaviorOperator<T, Unit> Throttle<T>(TimeSpan interval) {
    ExceptionsHelper.ThrowIfLessOrEqual(interval, TimeSpan.Zero, nameof(interval));
    return new ThrottleOperator<T>(interval);
  }

  /// <summary>
  /// Creates an execution operator from a delegate.
  /// </summary>
  /// <param name="operation">Delegate implementing operator behavior.</param>
  /// <returns>Execution operator wrapping <paramref name="operation"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="operation"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ExecutionOperator<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Func<T1, IExecutor<T2, T3>, T4> operation) {
    ExceptionsHelper.ThrowIfNull(operation, nameof(operation));
    return new AnonymousOperator<T1, T2, T3, T4>(operation);
  }

}