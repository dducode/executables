namespace Executables.Core.Executors;

internal sealed class AsyncOrElseExecutor<T1, T2>(IAsyncExecutor<T1, Optional<T2>> first, IAsyncExecutor<T1, T2> second) : IAsyncExecutor<T1, T2> {

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    Optional<T2> result = await first.Execute(input, token);
    return result.HasValue ? result.Value : await second.Execute(input, token);
  }

}