using System.Collections;

namespace Executables.Enumeration;

public struct ManyArrayExecutor<T1, T2>(IExecutor<T1, T2[]> executor, IEnumerator<T1> source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;

  private int _state;
  private int _index = -1;
  private T2[] _innerSource;

  public bool MoveNext() {
    switch (_state) {
      case 1:
        goto state1;
    }

    state0:
    if (!source.MoveNext())
      return false;
    _innerSource = executor.Execute(source.Current);
    _state = 1;
    state1:
    if (++_index >= _innerSource.Length) {
      _index = -1;
      _state = 0;
      goto state0;
    }

    Current = _innerSource[_index];
    return true;
  }

  void IEnumerator.Reset() {
    throw new NotSupportedException();
  }

  public void Dispose() {
    source.Dispose();
  }

}