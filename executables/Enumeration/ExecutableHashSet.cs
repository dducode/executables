using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableHashSet<T1, T2>(IQuery<T1, T2> query, HashSet<T1> source) : IEnumerable<T2> {

  public HashSetExecutor<T1, T2> GetEnumerator() {
    return new HashSetExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}