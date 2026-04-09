using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableArray<T1, T2>(IExecutor<T1, T2> executor, T1[] source) : IEnumerable<T2> {

  public ArrayExecutor<T1, T2> GetEnumerator() {
    return new ArrayExecutor<T1, T2>(executor, source);
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}