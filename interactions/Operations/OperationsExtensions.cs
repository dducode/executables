using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Executables;
using Interactions.Core.Internal;

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

  [Pure]
  public static IExecutable<T1, T3> Apply<T1, T2, T3>(this IExecutable<T2> executable, ExecutionOperator<T1, T2, Unit, T3> executionOperator) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new ExecutableOperator<T1, T2, T3>(executionOperator, executable);
  }

  [Pure]
  public static IExecutable<T1, T3> Apply<T1, T2, T3>(this ExecutionOperator<T1, T2, Unit, T3> executionOperator, IExecutable<T2> executable) {
    return executable.Apply(executionOperator);
  }

  [Pure]
  public static IExecutable<T1> Apply<T1, T2, T3>(this IExecutable<T2, T3> executable, ExecutionOperator<T1, T2, T3, Unit> executionOperator) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new ExecutableOperator<T1, T2, T3, Unit>(executionOperator, executable).SkipReturnValue();
  }

  [Pure]
  public static IExecutable<T1> Apply<T1, T2, T3>(this ExecutionOperator<T1, T2, T3, Unit> executionOperator, IExecutable<T2, T3> executable) {
    return executable.Apply(executionOperator);
  }

  [Pure]
  public static IExecutable<T1> Apply<T1, T2>(this IExecutable<T2> executable, ExecutionOperator<T1, T2, Unit, Unit> executionOperator) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(executionOperator, nameof(executionOperator));
    return new ExecutableOperator<T1, T2, Unit>(executionOperator, executable).SkipReturnValue();
  }

  [Pure]
  public static IExecutable<T1> Apply<T1, T2>(this ExecutionOperator<T1, T2, Unit, Unit> executionOperator, IExecutable<T2> executable) {
    return executable.Apply(executionOperator);
  }

}