using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableList<T1, T2>(IExecutor<T1, T2> executor, List<T1> source) : IEnumerable<T2> {

  public ListExecutor<T1, T2> GetEnumerator() {
    return new ListExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}