using System.Runtime.ExceptionServices;

namespace Interactions.Core.Events;

public interface IEvent<in T> {

  void Publish(T input);

}

public class Event<T> : Handleable<T, Unit>, IEvent<T> {

  private readonly Dictionary<Handler<T, Unit>, Subscriber> _handlers = new();

  public void Publish(T input) {
    using ListPool<Subscriber>.ListHandle subscribers = ListPool<Subscriber>.Get();
    lock (_handlers) {
      if (_handlers.Count == 0)
        return;
      subscribers.AddRange(_handlers.Values);
    }

    using ListPool<Exception>.ListHandle exceptions = ListPool<Exception>.Get();

    foreach (Subscriber subscriber in subscribers) {
      try {
        subscriber.Receive(input);
      }
      catch (Exception e) {
        exceptions.Add(e);
      }
    }

    switch (exceptions.Count) {
      case > 1:
        throw new AggregateException(exceptions);
      case 1:
        ExceptionDispatchInfo.Capture(exceptions.Single()).Throw();
        break;
    }
  }

  public override IDisposable Handle(Handler<T, Unit> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    Subscriber subscriber;

    lock (_handlers) {
      if (_handlers.ContainsKey(handler))
        throw new InvalidOperationException($"Already contains {handler}");
      subscriber = new Subscriber(this, handler);
      _handlers.Add(handler, subscriber);
    }

    return subscriber;
  }

  private void RemoveHandler(Handler<T, Unit> handler) {
    lock (_handlers)
      _handlers.Remove(handler);
  }

  private class Subscriber(Event<T> parent, Handler<T, Unit> handler) : IDisposable {

    private int _disposed;

    public void Receive(T input) {
      handler.Handle(input);
    }

    public void Dispose() {
      if (Interlocked.Exchange(ref _disposed, 1) != 0)
        return;

      parent.RemoveHandler(handler);
    }

  }

}