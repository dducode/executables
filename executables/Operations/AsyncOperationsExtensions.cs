using System.Diagnostics.Contracts;
using Executables.Core.Operators;
using Executables.Internal;

namespace Executables.Operations;

public static class AsyncOperationsExtensions {

  /// <summary>
  /// Applies an asynchronous execution operator to an executable.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="executionOperator">Operator to apply.</param>
  /// <returns>Wrapped executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executionOperator"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Apply<T1, T2, T3, T4>(
    this IAsyncExecutable<T2, T3> executable,
    AsyncExecutionOperator<T1, T2, T3, T4> executionOperator) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new AsyncExecutableOperator<T1, T2, T3, T4>(executionOperator, executable);
  }

  /// <summary>
  /// Applies an asynchronous execution operator to an executable.
  /// </summary>
  /// <param name="executionOperator">Operator to apply.</param>
  /// <param name="executable">Source executable.</param>
  /// <returns>Wrapped executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executable"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Apply<T1, T2, T3, T4>(
    this AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
    IAsyncExecutable<T2, T3> executable) {
    executionOperator.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncExecutableOperator<T1, T2, T3, T4>(executionOperator, executable);
  }

}