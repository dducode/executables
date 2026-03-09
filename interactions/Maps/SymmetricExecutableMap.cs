using Interactions.Core;

namespace Interactions.Maps;

internal sealed class SymmetricExecutableMap<T1, T2>(SymmetricMap<T1, T2> map, IExecutable<T2, T2> executable) : IExecutable<T1, T1> {

  public T1 Execute(T1 input) {
    return map.Invoke(input, executable);
  }

}