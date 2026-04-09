using System.Collections;

namespace Executables.Enumeration;

public readonly struct ExecutableQueue<T1, T2>(IExecutor<T1, T2> executor, Queue<T1> source) : IEnumerable<T2> {

  public QueueExecutor<T1, T2> GetEnumerator() {
    return new QueueExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}