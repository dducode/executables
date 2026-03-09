using Interactions.Core;

namespace Interactions.Maps;

internal sealed class AsyncSymmetricExecutableMap<T1, T2>(AsyncSymmetricMap<T1, T2> map, IAsyncExecutable<T2, T2> executable) : IAsyncExecutable<T1, T1> {

  public ValueTask<T1> Execute(T1 input, CancellationToken token = default) {
    return map.Invoke(input, executable, token);
  }

}