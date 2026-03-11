using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ExecutableStack<T1, T2>(IExecutable<T1, T2> executable, Stack<T1> source) : IEnumerable<T2> {

  public StackExecutor<T1, T2> GetEnumerator() {
    return new StackExecutor<T1, T2>(executable, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}