using Interactions.Core;

namespace Interactions.Operations;

internal sealed class ExecutableOperator<T1, T2, T3, T4>(
  ExecutionOperator<T1, T2, T3, T4> executionOperator,
  IExecutable<T2, T3> executable) : IExecutable<T1, T4> {

  public T4 Execute(T1 input) {
    return executionOperator.Invoke(input, executable);
  }

}