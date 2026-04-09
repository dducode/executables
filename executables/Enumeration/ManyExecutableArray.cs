using System.Collections;

namespace Executables.Enumeration;

public readonly struct ManyExecutableArray<T1, T2>(IExecutor<T1, T2[]> executor, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyArrayExecutor<T1, T2> GetEnumerator() {
    return new ManyArrayExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}