using Interactions.Operations;

namespace Interactions.Core.Operators;

internal sealed class ExecutableOperator<T1, T2, T3, T4>(
  ExecutionOperator<T1, T2, T3, T4> executionOperator,
  IExecutable<T2, T3> executable) : IExecutable<T1, T4>, IExecutor<T1, T4> {

  private readonly IExecutor<T2, T3> _executor = executable.GetExecutor();

  IExecutor<T1, T4> IExecutable<T1, T4>.GetExecutor() {
    return this;
  }

  T4 IExecutor<T1, T4>.Execute(T1 input) {
    return executionOperator.Invoke(input, _executor);
  }

}