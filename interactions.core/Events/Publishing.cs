using System.Collections;
using Interactions.Core.Internal;
using Interactions.Core.Subscribers;

namespace Interactions.Core.Events;

public struct Publishing<T>(T arg, List<ISubscriber<T>> subscribers) : IEnumerable<ISubscriber<T>> {

  public readonly T arg = arg;

  private bool _reversed;

  public Publishing<T> Reverse() {
    _reversed = !_reversed;
    return this;
  }

  public Enumerator GetEnumerator() {
    return new Enumerator(subscribers, _reversed);
  }

  IEnumerator<ISubscriber<T>> IEnumerable<ISubscriber<T>>.GetEnumerator() {
    return GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

  public struct Enumerator(List<ISubscriber<T>> subscribers, bool reversed) : IEnumerator<ISubscriber<T>> {

    public ISubscriber<T> Current => subscribers[_index];
    object IEnumerator.Current => subscribers[_index];

    private readonly ListHandle<ISubscriber<T>> _handle = new(subscribers);
    private int _index = reversed ? subscribers.Count : -1;

    public bool MoveNext() {
      if (!reversed)
        return ++_index < subscribers.Count;
      return --_index >= 0;
    }

    public void Reset() {
      if (!reversed)
        _index = -1;
      else
        _index = subscribers.Count;
    }

    public void Dispose() {
      Reset();
      _handle.Dispose();
    }

  }

}