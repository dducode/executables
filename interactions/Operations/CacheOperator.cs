using Interactions.Core;
using Interactions.Policies;

namespace Interactions.Operations;

internal sealed class CacheOperator<T1, T2>(ICacheStorage<T1, T2> storage) : BehaviorOperator<T1, T2> {

  private readonly object _lock = new();

  public override T2 Invoke(T1 input, IExecutable<T1, T2> next) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = next.Execute(input);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}