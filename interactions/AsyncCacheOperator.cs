using Interactions.Core;
using Interactions.Policies;

namespace Interactions;

public class AsyncCacheOperator<T1, T2>(ICacheStorage<T1, T2> storage) : AsyncBehaviorOperator<T1, T2> {

  private readonly object _lock = new();

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutable<T1, T2> next, CancellationToken token = default) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = await next.Execute(input, token);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}