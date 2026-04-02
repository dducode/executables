using Executables.Operations;

namespace Executables.Core.Operators;

internal sealed class AsyncResultFlattenOperator<T1, T2> : AsyncBehaviorOperator<T1, Result<T2>> {

  internal static AsyncResultFlattenOperator<T1, T2> Instance { get; } = new();

  private AsyncResultFlattenOperator() { }

  public override async ValueTask<Result<T2>> Invoke(T1 input, IAsyncExecutor<T1, Result<T2>> executor, CancellationToken token = default) {
    try {
      return await executor.Execute(input, token);
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}