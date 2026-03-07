using Interactions.Core;

namespace Interactions.Policies;

internal sealed class AsyncCachePolicy<T1, T2>(ICacheStorage<T1, T2> storage) : AsyncPolicy<T1, T2> {

  private readonly object _lock = new();

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token = default) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = await executable.Execute(input, token);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}