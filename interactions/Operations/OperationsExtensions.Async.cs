using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Operations;

public static partial class OperationsExtensions {

  [Pure]
  public static IAsyncExecutable<T1, T4> Apply<T1, T2, T3, T4>(
    this IAsyncExecutable<T2, T3> executable,
    AsyncExecutionOperator<T1, T2, T3, T4> executionOperator) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new AsyncExecutableOperator<T1, T2, T3, T4>(executionOperator, executable);
  }

  [Pure]
  public static IAsyncExecutable<T1, T4> Apply<T1, T2, T3, T4>(
    this AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
    IAsyncExecutable<T2, T3> executable) {
    return executable.Apply(executionOperator);
  }

}