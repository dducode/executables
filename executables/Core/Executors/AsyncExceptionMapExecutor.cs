namespace Executables.Core.Executors;

internal sealed class AsyncExceptionMapExecutor<T1, T2, TFrom>(IAsyncExecutor<T1, T2> executor, Func<TFrom, Exception> map)
  : IAsyncExecutor<T1, T2> where TFrom : Exception {

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    try {
      return await executor.Execute(input, token);
    }
    catch (TFrom ex) {
      throw map(ex);
    }
  }

}