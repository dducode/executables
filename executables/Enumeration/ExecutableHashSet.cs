using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableHashSet<T1, T2>(IExecutor<T1, T2> executor, HashSet<T1> source) : IEnumerable<T2> {

  public HashSetExecutor<T1, T2> GetEnumerator() {
    return new HashSetExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}