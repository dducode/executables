using Executables.Operations;

namespace Executables.Core.Executors;

internal sealed class AsyncOperatedExecutor<T1, T2, T3, T4>(IAsyncExecutor<T2, T3> executor, AsyncExecutionOperator<T1, T2, T3, T4> executionOperator)
  : IAsyncExecutor<T1, T4> {

  ValueTask<T4> IAsyncExecutor<T1, T4>.Execute(T1 input, CancellationToken token) {
    return executionOperator.Invoke(input, executor, token);
  }

}