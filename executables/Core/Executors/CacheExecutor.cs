using Executables.Operations;

namespace Executables.Core.Executors;

internal sealed class CacheExecutor<T1, T2>(IExecutor<T1, T2> executor, ICacheStorage<T1, T2> storage) : IExecutor<T1, T2> {

  private readonly object _lock = new();

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = executor.Execute(input);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}