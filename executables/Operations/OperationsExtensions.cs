using System.Diagnostics.Contracts;
using Executables.Core.Executors;
using Executables.Internal;

namespace Executables.Operations;

/// <summary>
/// Extension methods for applying execution operators.
/// </summary>
public static class OperationsExtensions {

  /// <summary>
  /// Applies an execution operator to an executor.
  /// </summary>
  /// <param name="executor">Source executor.</param>
  /// <param name="executionOperator">Operator to apply.</param>
  /// <returns>Wrapped executor.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executionOperator"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T4> Apply<T1, T2, T3, T4>(this IExecutor<T2, T3> executor, ExecutionOperator<T1, T2, T3, T4> executionOperator) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new OperatedExecutor<T1, T2, T3, T4>(executor, executionOperator);
  }

  /// <summary>
  /// Applies an execution operator to an executor.
  /// </summary>
  /// <param name="executionOperator">Operator to apply.</param>
  /// <param name="executor">Source executor.</param>
  /// <returns>Wrapped executor.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executor"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutor<T1, T4> Apply<T1, T2, T3, T4>(this ExecutionOperator<T1, T2, T3, T4> executionOperator, IExecutor<T2, T3> executor) {
    executionOperator.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return new OperatedExecutor<T1, T2, T3, T4>(executor, executionOperator);
  }

}