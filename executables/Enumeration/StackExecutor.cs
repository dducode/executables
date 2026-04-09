using System.Collections;

namespace Executables.Enumeration;

public struct StackExecutor<T1, T2>(IExecutor<T1, T2> executor, Stack<T1>.Enumerator source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;
  private Stack<T1>.Enumerator _source = source;

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = executor.Execute(_source.Current);
    return true;
  }

  void IEnumerator.Reset() {
    throw new NotSupportedException();
  }

  public void Dispose() {
    _source.Dispose();
  }

}