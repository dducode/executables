using System.Diagnostics.Contracts;
using Interactions.Core.Operators;
using Interactions.Internal;

namespace Interactions.Operations;

public static partial class OperationsExtensions {

  [Pure]
  public static IExecutable<T1, T4> Apply<T1, T2, T3, T4>(this IExecutable<T2, T3> executable, ExecutionOperator<T1, T2, T3, T4> executionOperator) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new ExecutableOperator<T1, T2, T3, T4>(executionOperator, executable);
  }

  [Pure]
  public static IExecutable<T1, T4> Apply<T1, T2, T3, T4>(this ExecutionOperator<T1, T2, T3, T4> executionOperator, IExecutable<T2, T3> executable) {
    return executable.Apply(executionOperator);
  }

}