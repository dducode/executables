using System.Collections;

namespace Interactions.Enumeration;

public readonly struct ManyExecutableStack<T1, T2>(IQuery<T1, Stack<T2>> query, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyStackExecutor<T1, T2> GetEnumerator() {
    return new ManyStackExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}