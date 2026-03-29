using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class AsyncExecutableOperator<T1, T2, T3, T4>(
  AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
  IAsyncExecutable<T2, T3> executable) : IAsyncExecutable<T1, T4>, IAsyncExecutor<T1, T4> {

  private readonly IAsyncExecutor<T2, T3> _executor = executable.GetExecutor();

  IAsyncExecutor<T1, T4> IAsyncExecutable<T1, T4>.GetExecutor() {
    return this;
  }

  ValueTask<T4> IAsyncExecutor<T1, T4>.Execute(T1 input, CancellationToken token) {
    return executionOperator.Invoke(input, _executor, token);
  }

}