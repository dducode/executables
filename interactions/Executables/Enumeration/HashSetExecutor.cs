using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public struct HashSetExecutor<T1, T2>(IExecutable<T1, T2> executable, HashSet<T1>.Enumerator source) : IEnumerator<T2> {

  private HashSet<T1>.Enumerator _source = source;

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = executable.Execute(_source.Current);
    return true;
  }

  public void Reset() {
    ((IEnumerator)_source).Reset();
  }

  public void Dispose() {
    _source.Dispose();
  }

}