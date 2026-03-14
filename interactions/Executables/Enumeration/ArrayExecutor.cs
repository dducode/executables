using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public struct ArrayExecutor<T1, T2>(IQuery<T1, T2> query, T1[] source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;
  private int _index = -1;

  public bool MoveNext() {
    if (++_index >= source.Length)
      return false;
    Current = query.Send(source[_index]);
    return true;
  }

  public void Reset() {
    _index = -1;
  }

  public void Dispose() {
    Reset();
  }

}