using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ExecutableList<T1, T2>(IQuery<T1, T2> query, List<T1> source) : IEnumerable<T2> {

  public ListExecutor<T1, T2> GetEnumerator() {
    return new ListExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}