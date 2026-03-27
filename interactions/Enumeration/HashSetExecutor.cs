using System.Collections;

namespace Interactions.Enumeration;

public struct HashSetExecutor<T1, T2>(IQuery<T1, T2> query, HashSet<T1>.Enumerator source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;
  private HashSet<T1>.Enumerator _source = source;

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = query.Send(_source.Current);
    return true;
  }

  void IEnumerator.Reset() {
    throw new NotSupportedException();
  }

  public void Dispose() {
    _source.Dispose();
  }

}