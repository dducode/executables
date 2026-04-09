using System.Collections;

namespace Executables.Enumeration;

public struct EnumerableExecutor<T1, T2>(IExecutor<T1, T2> executor, IEnumerator<T1> source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;

  public bool MoveNext() {
    if (!source.MoveNext())
      return false;
    Current = executor.Execute(source.Current);
    return true;
  }

  void IEnumerator.Reset() {
    throw new NotSupportedException();
  }

  public void Dispose() {
    source.Dispose();
  }

}