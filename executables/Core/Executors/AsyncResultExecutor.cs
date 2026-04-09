namespace Executables.Core.Executors;

internal sealed class AsyncResultExecutor<T1, T2>(IAsyncExecutor<T1, T2> executor) : IAsyncExecutor<T1, Result<T2>> {

  async ValueTask<Result<T2>> IAsyncExecutor<T1, Result<T2>>.Execute(T1 input, CancellationToken token) {
    try {
      return Result<T2>.FromResult(await executor.Execute(input, token));
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}