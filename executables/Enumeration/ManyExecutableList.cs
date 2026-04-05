using System.Collections;

namespace Executables.Enumeration;

public readonly struct ManyExecutableList<T1, T2>(IExecutor<T1, List<T2>> executor, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyListExecutor<T1, T2> GetEnumerator() {
    return new ManyListExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}