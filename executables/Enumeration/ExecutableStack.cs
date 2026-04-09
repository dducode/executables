using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableStack<T1, T2>(IExecutor<T1, T2> executor, Stack<T1> source) : IEnumerable<T2> {

  public StackExecutor<T1, T2> GetEnumerator() {
    return new StackExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}