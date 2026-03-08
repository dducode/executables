using Interactions.Core.Events;
using Interactions.Core.Internal;
using Interactions.Core.Subscribers;

namespace Interactions.Core;

public interface IEvent<T> : IExecutable<T, Unit> {

  IDisposable Subscribe(ISubscriber<T> subscriber);

}

public class Event<T> : Handleable<Publishing<T>, Unit>, IEvent<T> {

  private readonly List<ISubscriber<T>> _subscribers = [];
  private readonly object _lock = new();

  public Unit Execute(T input) {
    List<ISubscriber<T>> subscribers = Pool<List<ISubscriber<T>>>.Get();
    using var handle = new ListHandle<ISubscriber<T>>(subscribers);

    lock (_lock) {
      if (_subscribers.Count == 0)
        return default;
      subscribers.AddRange(_subscribers);
    }

    Handler<Publishing<T>, Unit> handler = Handler;
    if (handler == null)
      throw new MissingHandlerException("Cannot handle event");
    handler.Execute(new Publishing<T>(input, subscribers));
    return default;
  }

  public IDisposable Subscribe(ISubscriber<T> subscriber) {
    ExceptionsHelper.ThrowIfNull(subscriber, nameof(subscriber));

    lock (_lock) {
      if (_subscribers.Contains(subscriber))
        throw new InvalidOperationException($"Already contains {subscriber}");
      _subscribers.Add(subscriber);
    }

    return new SubscriberNode(this, subscriber);
  }

  private void RemoveSubscriber(ISubscriber<T> subscriber) {
    lock (_lock)
      _subscribers.Remove(subscriber);
  }

  private class SubscriberNode(Event<T> parent, ISubscriber<T> subscriber) : IDisposable {

    private int _disposed;

    public void Dispose() {
      if (Interlocked.Exchange(ref _disposed, 1) != 0)
        return;

      parent.RemoveSubscriber(subscriber);
    }

  }

}