using Interactions.Core;

namespace Interactions.Policies;

internal sealed class CachePolicy<T1, T2>(ICacheStorage<T1, T2> storage) : Policy<T1, T2> {

  private readonly object _lock = new();

  public override T2 Invoke(T1 input, IExecutable<T1, T2> executable) {
    lock (_lock)
      if (storage.TryGetValue(input, out T2 cached))
        return cached;

    T2 output = executable.Execute(input);
    lock (_lock)
      storage.Add(input, output);
    return output;
  }

}