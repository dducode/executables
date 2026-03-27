using System.Collections;

namespace Interactions.Enumeration;

public readonly struct ExecutableQueue<T1, T2>(IQuery<T1, T2> query, Queue<T1> source) : IEnumerable<T2> {

  public QueueExecutor<T1, T2> GetEnumerator() {
    return new QueueExecutor<T1, T2>(query, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}