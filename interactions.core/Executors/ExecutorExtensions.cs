using Interactions.Core.Internal;

namespace Interactions.Core.Executors;

public static partial class ExecutorExtensions {

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