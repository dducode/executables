using System.Collections;
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public struct ArrayExecutor<T1, T2>(IExecutable<T1, T2> executable, T1[] source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;
  private int _index = -1;

  public bool MoveNext() {
    if (++_index >= source.Length)
      return false;
    Current = executable.Execute(source[_index]);
    return true;
  }

  public void Reset() {
    _index = -1;
  }

  public void Dispose() {
    Reset();
  }

}