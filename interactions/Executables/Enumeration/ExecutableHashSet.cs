using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ExecutableHashSet<T1, T2>(IExecutable<T1, T2> executable, HashSet<T1> source) : IEnumerable<T2> {

  public HashSetExecutor<T1, T2> GetEnumerator() {
    return new HashSetExecutor<T1, T2>(executable, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}