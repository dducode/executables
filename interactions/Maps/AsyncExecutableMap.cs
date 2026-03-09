using Interactions.Core;

namespace Interactions.Maps;

internal sealed class AsyncExecutableMap<T1, T2, T3, T4>(AsyncMap<T1, T2, T3, T4> map, IAsyncExecutable<T2, T3> executable) : IAsyncExecutable<T1, T4> {

  public ValueTask<T4> Execute(T1 input, CancellationToken token = default) {
    return map.Invoke(input, executable, token);
  }

}