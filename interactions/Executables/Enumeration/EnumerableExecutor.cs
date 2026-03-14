using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public struct EnumerableExecutor<T1, T2>(IQuery<T1, T2> query, IEnumerator<T1> source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;

  public bool MoveNext() {
    if (!source.MoveNext())
      return false;
    Current = query.Send(source.Current);
    return true;
  }

  public void Reset() {
    source.Reset();
  }

  public void Dispose() {
    source.Dispose();
  }

}