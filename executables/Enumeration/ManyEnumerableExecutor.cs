using System.Collections;

namespace Executables.Enumeration;

public struct ManyEnumerableExecutor<T1, T2>(IQuery<T1, IEnumerable<T2>> query, IEnumerator<T1> source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;

  private int _state;
  private IEnumerator<T2> _innerSource;

  public bool MoveNext() {
    switch (_state) {
      case 1:
        goto state1;
    }

    state0:
    if (!source.MoveNext())
      return false;
    _innerSource = query.Send(source.Current).GetEnumerator();
    _state = 1;
    state1:
    if (!_innerSource.MoveNext()) {
      _innerSource.Dispose();
      _state = 0;
      goto state0;
    }

    Current = _innerSource.Current;
    return true;
  }

  void IEnumerator.Reset() {
    throw new NotSupportedException();
  }

  public void Dispose() {
    _innerSource?.Dispose();
    source.Dispose();
  }

}