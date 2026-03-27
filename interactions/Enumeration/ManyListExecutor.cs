using System.Collections;

namespace Interactions.Enumeration;

public struct ManyListExecutor<T1, T2>(IQuery<T1, List<T2>> query, IEnumerator<T1> source) : IEnumerator<T2> {

  public T2 Current { get; private set; }
  object IEnumerator.Current => Current;

  private int _state;
  private List<T2>.Enumerator _innerSource;

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
    _innerSource.Dispose();
    source.Dispose();
  }

}