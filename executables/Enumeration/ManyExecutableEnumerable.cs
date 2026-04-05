using System.Collections;

namespace Executables.Enumeration;

public readonly struct ManyExecutableEnumerable<T1, T2>(IExecutor<T1, IEnumerable<T2>> executor, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyEnumerableExecutor<T1, T2> GetEnumerator() {
    return new ManyEnumerableExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}