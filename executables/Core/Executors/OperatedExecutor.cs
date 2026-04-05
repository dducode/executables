using Executables.Operations;

namespace Executables.Core.Executors;

internal sealed class OperatedExecutor<T1, T2, T3, T4>(IExecutor<T2, T3> executor, ExecutionOperator<T1, T2, T3, T4> executionOperator) : IExecutor<T1, T4> {

  T4 IExecutor<T1, T4>.Execute(T1 input) {
    return executionOperator.Invoke(input, executor);
  }

}