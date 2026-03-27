using Interactions.Events;
using Interactions.Handling;
using Interactions.Internal;
using Interactions.Subscribers;

namespace Interactions;

public interface IEvent<T> : IExecutable<T, Unit> {

  void Publish(T input);
  IDisposable Subscribe(ISubscriber<T> subscriber);

}

public class Event<T> : Handleable<Publishing<T>, Unit>, IEvent<T> {

  private readonly HashSet<ISubscriber<T>> _subscribers = [];
  private readonly object _lock = new();

  public virtual void Publish(T input) {
    List<ISubscriber<T>> subscribers = Pool<List<ISubscriber<T>>>.Get();
    using var handle = new ListHandle<ISubscriber<T>>(subscribers);

    lock (_lock) {
      if (_subscribers.Count == 0)
        return;
      subscribers.AddRange(_subscribers);
    }

    Handler<Publishing<T>, Unit> handler = Handler;
    if (handler == null)
      throw new MissingHandlerException("Cannot handle event");
    handler.Handle(new Publishing<T>(input, subscribers));
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IExecutor<T, Unit> IExecutable<T, Unit>.GetExecutor() {
    return GetExecutor();
  }

  public IDisposable Subscribe(ISubscriber<T> subscriber) {
    ExceptionsHelper.ThrowIfNull(subscriber, nameof(subscriber));

    lock (_lock)
      if (!_subscribers.Add(subscriber))
        throw new InvalidOperationException($"Already contains {subscriber}");

    return new SubscriberNode(this, subscriber);
  }

  private void RemoveSubscriber(ISubscriber<T> subscriber) {
    lock (_lock)
      _subscribers.Remove(subscriber);
  }

  public readonly struct Executor(IEvent<T> e) : IExecutor<T, Unit> {

    public Unit Execute(T input) {
      e.Publish(input);
      return default;
    }

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