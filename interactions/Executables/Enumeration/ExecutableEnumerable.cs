using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ExecutableEnumerable<T1, T2>(IExecutable<T1, T2> executable, IEnumerable<T1> source) : IEnumerable<T2> {

  public EnumerableExecutor<T1, T2> GetEnumerator() {
    return new EnumerableExecutor<T1, T2>(executable, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}