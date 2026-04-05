using System.Diagnostics.Contracts;
using Executables.Core.Operators;
using Executables.Internal;

namespace Executables.Operations;

/// <summary>
/// Factory methods for creating execution operators.
/// </summary>
public static class ExecutionOperator {

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