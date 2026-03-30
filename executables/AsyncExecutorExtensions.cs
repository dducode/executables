using Executables.Core.Executors;
using Executables.Internal;

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