namespace Executables.Core.Executors;

internal sealed class AsyncTransitiveExecutor<T1, T2>(IAsyncExecutor<T1, T2> executor, AsyncAction<T2> action) : IAsyncExecutor<T1, T2> {

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    T2 result = await executor.Execute(input, token);
    await action(result, token);
    return result;
  }

}