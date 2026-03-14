using Interactions.Core;

namespace Interactions.Operations;

internal sealed class AsyncExecutableOperator<T1, T2, T3, T4>(
  AsyncExecutionOperator<T1, T2, T3, T4> executionOperator,
  IAsyncExecutable<T2, T3> executable) : IAsyncExecutable<T1, T4> {

  public ValueTask<T4> Execute(T1 input, CancellationToken token = default) {
    return executionOperator.Invoke(input, executable, token);
  }

}