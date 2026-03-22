using Interactions.Core;

namespace Interactions.Operations;

internal sealed class AsyncAnonymousOperator<T1, T2, T3, T4>(AsyncFunc<T1, IAsyncExecutor<T2, T3>, T4> operation) : AsyncExecutionOperator<T1, T2, T3, T4> {

  public override ValueTask<T4> Invoke(T1 input, IAsyncExecutor<T2, T3> executor, CancellationToken token = default) {
    return operation(input, executor, token);
  }

}