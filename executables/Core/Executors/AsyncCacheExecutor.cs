using Executables.Operations;

namespace Executables.Core.Executors;

internal sealed class AsyncCacheExecutor<T1, T2>(IAsyncExecutor<T1, T2> executor, ICacheStorage<T1, T2> storage) : IAsyncExecutor<T1, T2> {

  private readonly object _lock = new();

  ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return new ValueTask<T2>(cached);

    return Await(input, token);
  }

  private async ValueTask<T2> Await(T1 input, CancellationToken token) {
    T2 output = await executor.Execute(input, token);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}