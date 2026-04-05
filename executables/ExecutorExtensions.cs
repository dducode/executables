using System.Diagnostics.Contracts;
using Executables.Analytics;
using Executables.Context;
using Executables.Core.Executors;
using Executables.Executors;
using Executables.Internal;
using Executables.Operations;
using Executables.Policies;

namespace Executables;

public static class ExecutorExtensions {

  /// <summary>
  /// Executes a parameterless executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static T Execute<T>(this IExecutor<Unit, T> executor) {
    return executor.Execute(default);
  }

  /// <summary>
  /// Executes a parameterless executor with no result.
  /// </summary>
  public static void Execute(this IExecutor<Unit, Unit> executor) {
    executor.Execute(default);
  }

  /// <summary>
  /// Runs a side effect on each execution result while returning the original result unchanged.
  /// </summary>
  /// <param name="executor">Executor producing the observed result.</param>
  /// <param name="action">Side-effect action invoked with the produced result.</param>
  /// <returns>Executor that preserves the original result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> Tap<T1, T2>(this IExecutor<T1, T2> executor, Action<T2> action) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new TransitiveExecutor<T1, T2>(executor, action);
  }

  /// <summary>
  /// Executes an executor within a newly initialized interaction context.
  /// </summary>
  /// <param name="executor">Executor to run inside the context.</param>
  /// <param name="init">Context initialization logic.</param>
  /// <returns>Executor wrapped with contextual execution.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="init"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> WithContext<T1, T2>(this IExecutor<T1, T2> executor, ContextInit init) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(init, nameof(init));
    return new ContextExecutor<T1, T2>(executor, init);
  }

  /// <summary>
  /// Builds and applies a policy pipeline to an executor.
  /// </summary>
  /// <param name="executor">Executor to wrap with policies.</param>
  /// <param name="building">Builder action that configures policies.</param>
  /// <returns>Executor wrapped with configured policies.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="building"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> WithPolicy<T1, T2>(this IExecutor<T1, T2> executor, Action<PolicyBuilder<T1, T2>> building) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(building, nameof(building));
    var builder = new PolicyBuilder<T1, T2>();
    building(builder);
    return builder.Apply(executor);
  }

  /// <summary>
  /// Wraps executor result into <see cref="Result{T}" /> capturing thrown exceptions.
  /// </summary>
  /// <param name="executor">Executor to wrap.</param>
  /// <returns>Executor returning success or failure result.</returns>
  [Pure]
  public static IExecutor<T1, Result<T2>> WithResult<T1, T2>(this IExecutor<T1, T2> executor) {
    executor.ThrowIfNullReference();
    return new ResultExecutor<T1, T2>(executor);
  }

  /// <summary>
  /// Wraps an executor already returning <see cref="Result{T}"/> so that thrown exceptions are also converted to <see cref="Result{T}"/>.
  /// </summary>
  /// <param name="executor">Executor returning <see cref="Result{T}"/>.</param>
  /// <returns>Executor that preserves returned results and converts thrown exceptions to failure results.</returns>
  [Pure]
  public static IExecutor<T1, Result<T2>> WithResult<T1, T2>(this IExecutor<T1, Result<T2>> executor) {
    executor.ThrowIfNullReference();
    return new ResultFlattenExecutor<T1, T2>(executor);
  }

  /// <summary>
  /// Starts configuration of exception suppression for executor results.
  /// </summary>
  /// <param name="executor">Executor to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionExecutorProvider<T1, T2> SuppressException<T1, T2>(this IExecutor<T1, T2> executor) {
    executor.ThrowIfNullReference();
    return new SuppressExceptionExecutorProvider<T1, T2>(executor);
  }

  /// <summary>
  /// Starts configuration of exception suppression for executors returning <see cref="Optional{T}"/>.
  /// </summary>
  /// <param name="executor">Executor to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionFlattenExecutorProvider<T1, T2> SuppressException<T1, T2>(this IExecutor<T1, Optional<T2>> executor) {
    executor.ThrowIfNullReference();
    return new SuppressExceptionFlattenExecutorProvider<T1, T2>(executor);
  }

  /// <summary>
  /// Pipes an executor through a transformation function.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="pipe">Transformation function.</param>
  /// <returns>Transformed executor.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="pipe"/> is <see langword="null"/>.</exception>
  [Pure]
  [Obsolete]
  public static IExecutor<T1, T3> Pipe<T1, T2, T3>(this IExecutor<T1, T2> executor, Func<IExecutor<T1, T2>, IExecutor<T1, T3>> pipe) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(pipe, nameof(pipe));
    return pipe(executor);
  }

  /// <summary>
  /// Adds cache behavior to an executor.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Executor with caching behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="storage"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> Cache<T1, T2>(this IExecutor<T1, T2> executor, ICacheStorage<T1, T2> storage) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new CacheExecutor<T1, T2>(executor, storage);
  }

  /// <summary>
  /// Adds metrics behavior to an executor.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag passed to all metrics callbacks.</param>
  /// <returns>Executor with metrics behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metrics"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> Metrics<T1, T2>(this IExecutor<T1, T2> executor, IMetrics<T1, T2> metrics, string tag = null) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new MetricsExecutor<T1, T2>(executor, metrics, tag);
  }

  /// <summary>
  /// Maps exceptions of a specific type thrown by an executor.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="map">Function that maps the caught exception to a new exception.</param>
  /// <returns>Executor with exception mapping behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T2> MapException<T1, T2, TFrom>(this IExecutor<T1, T2> executor, Func<TFrom, Exception> map) where TFrom : Exception {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return new ExceptionMapExecutor<T1, T2, TFrom>(executor, map);
  }

  /// <summary>
  /// Schedules executor calls on the thread pool.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <returns>Thread-pool scheduled executor.</returns>
  [Pure]
  public static IExecutor<T, Unit> OnThreadPool<T>(this IExecutor<T, Unit> executor) {
    executor.ThrowIfNullReference();
    return new ThreadPoolExecutor<T>(executor);
  }

  /// <summary>
  /// Throttles repeated executions within the specified interval.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="interval">Minimum interval between forwarded executions.</param>
  /// <returns>Executor with throttling behavior.</returns>
  /// <exception cref="ArgumentException"><paramref name="interval"/> is less than or equal to zero.</exception>
  [Pure]
  public static IExecutor<T, Unit> Throttle<T>(this IExecutor<T, Unit> executor, TimeSpan interval) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfLessOrEqual(interval, TimeSpan.Zero, nameof(interval));
    return new ThrottleExecutor<T>(executor, interval);
  }

  /// <summary>
  /// Partially applies the first argument of a two-argument executor.
  /// </summary>
  /// <returns>Executor waiting for the remaining argument.</returns>
  public static IExecutor<T2, T3> Execute<T1, T2, T3>(this IExecutor<(T1, T2), T3> executor, T1 t1) {
    executor.ThrowIfNullReference();
    return new PartialExecutor<T1, T2, T3>(executor, t1);
  }

  /// <summary>
  /// Executes a two-argument executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static T3 Execute<T1, T2, T3>(this IExecutor<(T1, T2), T3> executor, T1 t1, T2 t2) {
    return executor.Execute((t1, t2));
  }

  /// <summary>
  /// Partially applies the first argument of a three-argument executor.
  /// </summary>
  /// <returns>Executor waiting for two remaining arguments.</returns>
  public static IExecutor<(T2, T3), T4> Execute<T1, T2, T3, T4>(this IExecutor<(T1, T2, T3), T4> executor, T1 t1) {
    executor.ThrowIfNullReference();
    return new OneArgPartialExecutor<T1, T2, T3, T4>(executor, t1);
  }

  /// <summary>
  /// Partially applies the first two arguments of a three-argument executor.
  /// </summary>
  /// <returns>Executor waiting for the remaining argument.</returns>
  public static IExecutor<T3, T4> Execute<T1, T2, T3, T4>(this IExecutor<(T1, T2, T3), T4> executor, T1 t1, T2 t2) {
    executor.ThrowIfNullReference();
    return new TwoArgsPartialExecutor<T1, T2, T3, T4>(executor, t1, t2);
  }

  /// <summary>
  /// Executes a three-argument executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static T4 Execute<T1, T2, T3, T4>(this IExecutor<(T1, T2, T3), T4> executor, T1 t1, T2 t2, T3 t3) {
    return executor.Execute((t1, t2, t3));
  }

  /// <summary>
  /// Partially applies the first argument of a four-argument executor.
  /// </summary>
  /// <returns>Executor waiting for three remaining arguments.</returns>
  public static IExecutor<(T2, T3, T4), T5> Execute<T1, T2, T3, T4, T5>(this IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1) {
    executor.ThrowIfNullReference();
    return new OneArgPartialExecutor<T1, T2, T3, T4, T5>(executor, t1);
  }

  /// <summary>
  /// Partially applies the first two arguments of a four-argument executor.
  /// </summary>
  /// <returns>Executor waiting for two remaining arguments.</returns>
  public static IExecutor<(T3, T4), T5> Execute<T1, T2, T3, T4, T5>(this IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2) {
    executor.ThrowIfNullReference();
    return new TwoArgsPartialExecutor<T1, T2, T3, T4, T5>(executor, t1, t2);
  }

  /// <summary>
  /// Partially applies the first three arguments of a four-argument executor.
  /// </summary>
  /// <returns>Executor waiting for the remaining argument.</returns>
  public static IExecutor<T4, T5> Execute<T1, T2, T3, T4, T5>(this IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2, T3 t3) {
    executor.ThrowIfNullReference();
    return new ThreeArgsPartialExecutor<T1, T2, T3, T4, T5>(executor, t1, t2, t3);
  }

  /// <summary>
  /// Executes a four-argument executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static T5 Execute<T1, T2, T3, T4, T5>(this IExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2, T3 t3, T4 t4) {
    return executor.Execute((t1, t2, t3, t4));
  }

}