using System.Collections;

namespace Executables.Enumeration;

public readonly struct ManyExecutableQueue<T1, T2>(IExecutor<T1, Queue<T2>> executor, IEnumerable<T1> source) : IEnumerable<T2> {

  public ManyQueueExecutor<T1, T2> GetEnumerator() {
    return new ManyQueueExecutor<T1, T2>(executor, source.GetEnumerator());
  }

  IEnumerator<T2> IEnumerable<T2>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}