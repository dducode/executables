using Interactions.Core;

namespace Interactions.Policies;

internal sealed class CachePolicy<T1, T2>(ICacheStorage<T1, T2> storage) : Policy<T1, T2> {

  private readonly object _lock = new();

  public override T2 Execute(T1 input, IExecutable<T1, T2> executable) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = executable.Execute(input);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}

internal sealed class AsyncCachePolicy<T1, T2>(ICacheStorage<T1, T2> storage) : AsyncPolicy<T1, T2> {

  private readonly object _lock = new();

  public override async ValueTask<T2> Execute(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = await executable.Execute(input, token);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}