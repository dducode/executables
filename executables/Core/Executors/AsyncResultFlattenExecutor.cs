namespace Executables.Core.Executors;

internal sealed class AsyncResultFlattenExecutor<T1, T2>(IAsyncExecutor<T1, Result<T2>> executor) : IAsyncExecutor<T1, Result<T2>> {

  async ValueTask<Result<T2>> IAsyncExecutor<T1, Result<T2>>.Execute(T1 input, CancellationToken token) {
    try {
      return await executor.Execute(input, token);
    }
    catch (Exception e) {
      return Result<T2>.FromException(e);
    }
  }

}