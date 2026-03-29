using System.Collections;

namespace Executables.Enumeration;

public readonly struct ManyExecutableArray<T1, T2>(IQuery<T1, T2[]> query, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyArrayExecutor<T1, T2> GetEnumerator() {
    return new ManyArrayExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}