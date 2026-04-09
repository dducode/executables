using System.Diagnostics.Contracts;
using Executables.Core.Operators;
using Executables.Internal;

namespace Executables.Operations;

/// <summary>
/// Factory methods for creating asynchronous execution operators.
/// </summary>
public static class AsyncExecutionOperator {

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