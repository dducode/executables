using System.Diagnostics.Contracts;
using Executables.Analytics;
using Executables.Context;
using Executables.Core.Executors;
using Executables.Executors;
using Executables.Internal;
using Executables.Operations;
using Executables.Policies;

namespace Executables;

public static class AsyncExecutorExtensions {

  /// <summary>
  /// Executes a parameterless async executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static ValueTask<T> Execute<T>(this IAsyncExecutor<Unit, T> executor, CancellationToken token = default) {
    return executor.Execute(default, token);
  }

  /// <summary>
  /// Executes a parameterless async executor with no result.
  /// </summary>
  public static async ValueTask Execute(this IAsyncExecutor<Unit, Unit> executor, CancellationToken token = default) {
    await executor.Execute(default, token);
  }

  /// <summary>
  /// Runs an asynchronous side effect on each result while returning the original result unchanged.
  /// </summary>
  /// <param name="executor">Executor producing the observed result.</param>
  /// <param name="action">Asynchronous side-effect action invoked with the produced result.</param>
  /// <returns>Asynchronous executor that preserves the original result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> Tap<T1, T2>(this IAsyncExecutor<T1, T2> executor, AsyncAction<T2> action) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AsyncTransitiveExecutor<T1, T2>(executor, action);
  }

  /// <summary>
  /// Executes an asynchronous executor within a newly initialized interaction context.
  /// </summary>
  /// <param name="executor">Executor to run inside the context.</param>
  /// <param name="init">Context initialization logic.</param>
  /// <returns>Executor wrapped with contextual execution.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="init"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> WithContext<T1, T2>(this IAsyncExecutor<T1, T2> executor, ContextInit init) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(init, nameof(init));
    return new AsyncContextExecutor<T1, T2>(executor, init);
  }

  /// <summary>
  /// Builds and applies an asynchronous policy pipeline to an executor.
  /// </summary>
  /// <param name="executor">Executor to wrap with policies.</param>
  /// <param name="building">Builder action that configures policies.</param>
  /// <returns>Executor wrapped with configured policies.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="building"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> WithPolicy<T1, T2>(this IAsyncExecutor<T1, T2> executor, Action<AsyncPolicyBuilder<T1, T2>> building) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(building, nameof(building));
    var source = new AsyncPolicyBuilder<T1, T2>();
    building(source);
    return source.Apply(executor);
  }

  /// <summary>
  /// Wraps executor result into <see cref="Result{T}" /> capturing thrown exceptions.
  /// </summary>
  /// <param name="executor">Executor to wrap.</param>
  /// <returns>Executor returning success or failure result.</returns>
  [Pure]
  public static IAsyncExecutor<T1, Result<T2>> WithResult<T1, T2>(this IAsyncExecutor<T1, T2> executor) {
    executor.ThrowIfNullReference();
    return new AsyncResultExecutor<T1, T2>(executor);
  }

  /// <summary>
  /// Wraps an executor already returning <see cref="Result{T}"/> so that thrown exceptions are also converted to <see cref="Result{T}"/>.
  /// </summary>
  /// <param name="executor">Executor returning <see cref="Result{T}"/>.</param>
  /// <returns>Executor that preserves returned results and converts thrown exceptions to failure results.</returns>
  [Pure]
  public static IAsyncExecutor<T1, Result<T2>> WithResult<T1, T2>(this IAsyncExecutor<T1, Result<T2>> executor) {
    executor.ThrowIfNullReference();
    return new AsyncResultFlattenExecutor<T1, T2>(executor);
  }

  /// <summary>
  /// Starts configuration of exception suppression for asynchronous executor results.
  /// </summary>
  /// <param name="executor">Executor to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionAsyncExecutorProvider<T1, T2> SuppressException<T1, T2>(this IAsyncExecutor<T1, T2> executor) {
    executor.ThrowIfNullReference();
    return new SuppressExceptionAsyncExecutorProvider<T1, T2>(executor);
  }

  /// <summary>
  /// Starts configuration of exception suppression for asynchronous executors returning <see cref="Optional{T}"/>.
  /// </summary>
  /// <param name="executor">Executor to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionFlattenAsyncExecutorProvider<T1, T2> SuppressException<T1, T2>(this IAsyncExecutor<T1, Optional<T2>> executor) {
    executor.ThrowIfNullReference();
    return new SuppressExceptionFlattenAsyncExecutorProvider<T1, T2>(executor);
  }

  /// <summary>
  /// Adds asynchronous cache behavior to an executor.
  /// </summary>
  /// <param name="executor">Source asynchronous executor.</param>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Asynchronous executor with caching behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="storage"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> Cache<T1, T2>(this IAsyncExecutor<T1, T2> executor, ICacheStorage<T1, T2> storage) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new AsyncCacheExecutor<T1, T2>(executor, storage);
  }

  /// <summary>
  /// Adds asynchronous metrics behavior to an executor.
  /// </summary>
  /// <param name="executor">Source asynchronous executor.</param>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag passed to all metrics callbacks.</param>
  /// <returns>Asynchronous executor with metrics behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metrics"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> Metrics<T1, T2>(this IAsyncExecutor<T1, T2> executor, IMetrics<T1, T2> metrics, string tag = null) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new AsyncMetricsExecutor<T1, T2>(executor, metrics, tag);
  }

  /// <summary>
  /// Maps exceptions of a specific type thrown by an asynchronous executor.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="map">Function that maps the caught exception to a new exception.</param>
  /// <returns>Asynchronous executor with exception mapping behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T2> MapException<T1, T2, TFrom>(this IAsyncExecutor<T1, T2> executor, Func<TFrom, Exception> map) where TFrom : Exception {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return new AsyncExceptionMapExecutor<T1, T2, TFrom>(executor, map);
  }

  /// <summary>
  /// Partially applies the first argument of a two-argument async executor.
  /// </summary>
  /// <returns>Executor waiting for the remaining argument.</returns>
  public static IAsyncExecutor<T2, T3> Execute<T1, T2, T3>(this IAsyncExecutor<(T1, T2), T3> executor, T1 t1) {
    executor.ThrowIfNullReference();
    return new AsyncPartialExecutor<T1, T2, T3>(executor, t1);
  }

  /// <summary>
  /// Executes a two-argument async executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static ValueTask<T3> Execute<T1, T2, T3>(this IAsyncExecutor<(T1, T2), T3> executor, T1 t1, T2 t2, CancellationToken token = default) {
    return executor.Execute((t1, t2), token);
  }

  /// <summary>
  /// Partially applies the first argument of a three-argument async executor.
  /// </summary>
  /// <returns>Executor waiting for two remaining arguments.</returns>
  public static IAsyncExecutor<(T2, T3), T4> Execute<T1, T2, T3, T4>(this IAsyncExecutor<(T1, T2, T3), T4> executor, T1 t1) {
    executor.ThrowIfNullReference();
    return new OneArgAsyncPartialExecutor<T1, T2, T3, T4>(executor, t1);
  }

  /// <summary>
  /// Partially applies the first two arguments of a three-argument async executor.
  /// </summary>
  /// <returns>Executor waiting for the remaining argument.</returns>
  public static IAsyncExecutor<T3, T4> Execute<T1, T2, T3, T4>(this IAsyncExecutor<(T1, T2, T3), T4> executor, T1 t1, T2 t2) {
    executor.ThrowIfNullReference();
    return new TwoArgsAsyncPartialExecutor<T1, T2, T3, T4>(executor, t1, t2);
  }

  /// <summary>
  /// Executes a three-argument async executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static ValueTask<T4> Execute<T1, T2, T3, T4>(this IAsyncExecutor<(T1, T2, T3), T4> executor, T1 t1, T2 t2, T3 t3, CancellationToken token = default) {
    return executor.Execute((t1, t2, t3), token);
  }

  /// <summary>
  /// Partially applies the first argument of a four-argument async executor.
  /// </summary>
  /// <returns>Executor waiting for three remaining arguments.</returns>
  public static IAsyncExecutor<(T2, T3, T4), T5> Execute<T1, T2, T3, T4, T5>(this IAsyncExecutor<(T1, T2, T3, T4), T5> executor, T1 t1) {
    executor.ThrowIfNullReference();
    return new OneArgAsyncPartialExecutor<T1, T2, T3, T4, T5>(executor, t1);
  }

  /// <summary>
  /// Partially applies the first two arguments of a four-argument async executor.
  /// </summary>
  /// <returns>Executor waiting for two remaining arguments.</returns>
  public static IAsyncExecutor<(T3, T4), T5> Execute<T1, T2, T3, T4, T5>(this IAsyncExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2) {
    executor.ThrowIfNullReference();
    return new TwoArgsAsyncPartialExecutor<T1, T2, T3, T4, T5>(executor, t1, t2);
  }

  /// <summary>
  /// Partially applies the first three arguments of a four-argument async executor.
  /// </summary>
  /// <returns>Executor waiting for the remaining argument.</returns>
  public static IAsyncExecutor<T4, T5> Execute<T1, T2, T3, T4, T5>(this IAsyncExecutor<(T1, T2, T3, T4), T5> executor, T1 t1, T2 t2, T3 t3) {
    executor.ThrowIfNullReference();
    return new ThreeArgsAsyncPartialExecutor<T1, T2, T3, T4, T5>(executor, t1, t2, t3);
  }

  /// <summary>
  /// Executes a four-argument async executor.
  /// </summary>
  /// <returns>Execution result.</returns>
  public static ValueTask<T5> Execute<T1, T2, T3, T4, T5>(
    this IAsyncExecutor<(T1, T2, T3, T4), T5> executor,
    T1 t1,
    T2 t2,
    T3 t3,
    T4 t4,
    CancellationToken token = default) {
    return executor.Execute((t1, t2, t3, t4), token);
  }

}