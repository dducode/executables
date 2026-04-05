using System.Diagnostics.Contracts;
using Executables.Core.Executors;
using Executables.Internal;

namespace Executables.Operations;

public static class AsyncOperationsExtensions {

  /// <summary>
  /// Applies an asynchronous execution operator to an executor.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="executionOperator">Operator to apply.</param>
  /// <returns>Wrapped executor.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executionOperator"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T4> Apply<T1, T2, T3, T4>(this IAsyncExecutor<T2, T3> executor, AsyncExecutionOperator<T1, T2, T3, T4> executionOperator) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new AsyncOperatedExecutor<T1, T2, T3, T4>(executor, executionOperator);
  }

  /// <summary>
  /// Applies an asynchronous execution operator to an executor.
  /// </summary>
  /// <param name="executionOperator">Operator to apply.</param>
  /// <param name="executor">Source executor.</param>
  /// <returns>Wrapped executor.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executor"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutor<T1, T4> Apply<T1, T2, T3, T4>(this AsyncExecutionOperator<T1, T2, T3, T4> executionOperator, IAsyncExecutor<T2, T3> executor) {
    executionOperator.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return new AsyncOperatedExecutor<T1, T2, T3, T4>(executor, executionOperator);
  }

}