using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ExecutableArray<T1, T2>(IExecutable<T1, T2> executable, T1[] source) : IEnumerable<T2> {

  public ArrayExecutor<T1, T2> GetEnumerator() {
    return new ArrayExecutor<T1, T2>(executable, source);
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}