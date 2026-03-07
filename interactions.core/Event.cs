using Interactions.Core.Events;
using Interactions.Core.Internal;

namespace Interactions.Core;

public interface IEvent<T> : IExecutable<T, Unit> {

  IDisposable Subscribe(ISubscriber<T> subscriber);

}

public class Event<T> : Handleable<Publishing<T>, Unit>, IEvent<T> {

  private readonly List<ISubscriber<T>> _subscribers = [];
  private readonly object _lock = new();

  private HandlerNode _handlerNode;

  public Unit Execute(T input) {
    List<ISubscriber<T>> subscribers = Pool<List<ISubscriber<T>>>.Get();
    using var handle = new ListHandle<ISubscriber<T>>(subscribers);

    lock (_lock) {
      if (_subscribers.Count == 0)
        return default;
      subscribers.AddRange(_subscribers);
    }

    HandlerNode node = Volatile.Read(ref _handlerNode);
    if (node == null)
      throw new MissingHandlerException("Cannot handle event");
    node.Publish(input, subscribers);
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

  public override IDisposable Handle(Handler<Publishing<T>, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));

    lock (_lock) {
      if (_handlerNode != null)
        throw new InvalidOperationException("Already has handler");
      return _handlerNode = new HandlerNode(this, handler);
    }
  }

  private void RemoveSubscriber(ISubscriber<T> subscriber) {
    lock (_lock)
      _subscribers.Remove(subscriber);
  }

  private void RemoveHandler(HandlerNode node) {
    Interlocked.CompareExchange(ref _handlerNode, null, node);
  }

  private class SubscriberNode(Event<T> parent, ISubscriber<T> subscriber) : IDisposable {

    private int _disposed;

    public void Dispose() {
      if (Interlocked.Exchange(ref _disposed, 1) != 0)
        return;

      parent.RemoveSubscriber(subscriber);
    }

  }

  private class HandlerNode(Event<T> parent, Handler<Publishing<T>, Unit> handler) : IDisposable {

    private int _disposed;

    public void Publish(T arg, List<ISubscriber<T>> subscribers) {
      handler.Execute(new Publishing<T>(arg, subscribers));
    }

    public void Dispose() {
      if (Interlocked.Exchange(ref _disposed, 1) != 0)
        return;

      parent.RemoveHandler(this);
    }

  }

}