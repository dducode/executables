using Interactions.Core.Events;

namespace Interactions.Core.Extensions;

public static class EventExtensions {

  public static IDisposable Subscribe<T>(this IEvent<T> e, Action<T> action) {
    return e.Subscribe(Subscriber.Create(action));
  }

  public static IDisposable Subscribe(this IEvent<Unit> e, Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return e.Subscribe(Subscriber.Create<Unit>(_ => action()));
  }

  public static void SubscribeOnce<T>(this IEvent<T> e, Action<T> action) {
    var handle = new DisposeHandle();
    handle.Register(e.Subscribe(Subscriber.Create(action).Once(handle)));
  }

  public static void SubscribeOnce(this IEvent<Unit> e, Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    var handle = new DisposeHandle();
    handle.Register(e.Subscribe(Subscriber.Create<Unit>(_ => action()).Once(handle)));
  }

}