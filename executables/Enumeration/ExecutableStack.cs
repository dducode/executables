using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableStack<T1, T2>(IQuery<T1, T2> query, Stack<T1> source) : IEnumerable<T2> {

  public StackExecutor<T1, T2> GetEnumerator() {
    return new StackExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}