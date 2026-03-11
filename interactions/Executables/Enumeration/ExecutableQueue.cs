using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct ExecutableQueue<T1, T2>(IExecutable<T1, T2> executable, Queue<T1> source) : IEnumerable<T2> {

  public QueueExecutor<T1, T2> GetEnumerator() {
    return new QueueExecutor<T1, T2>(executable, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}