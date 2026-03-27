using Interactions.Operations;

namespace Interactions.Core.Operators;

internal sealed class AsyncResultOperator<T1, T2> : AsyncExecutionOperator<T1, T1, T2, Result<T2>> {

  public override async ValueTask<Result<T2>> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    try {
      return Result<T2>.FromResult(await executor.Execute(input, token));
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}