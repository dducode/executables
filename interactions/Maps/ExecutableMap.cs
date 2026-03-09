using Interactions.Core;

namespace Interactions.Maps;

internal sealed class ExecutableMap<T1, T2, T3, T4>(Map<T1, T2, T3, T4> map, IExecutable<T2, T3> executable) : IExecutable<T1, T4> {

  public T4 Execute(T1 input) {
    return map.Invoke(input, executable);
  }

}