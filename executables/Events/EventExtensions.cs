using Executables.Core.Subscribers;
using Executables.Internal;
using Executables.Lifecycle;
using Executables.Subscribers;

namespace Executables.Events;

/// <summary>
/// Extension methods for publishing to and subscribing to events.
/// </summary>
public static class EventExtensions {

  /// <summary>
  /// Publishes a notification.
  /// </summary>
  /// <param name="e">Event to publish to.</param>
  public static void Publish(this IEvent<Unit> e) {
    e.Publish(default);
  }

  /// <summary>
  /// Subscribes an action to an event.
  /// </summary>
  /// <typeparam name="T">Type of published event value.</typeparam>
  /// <param name="e">Event to subscribe to.</param>
  /// <param name="action">Action executed for each published value.</param>
  /// <returns>Disposable subscription handle.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  public static IDisposable Subscribe<T>(this IEvent<T> e, Action<T> action) {
    return e.Subscribe(Subscriber.Create(action));
  }

  /// <summary>
  /// Subscribes a parameterless action to a unit event.
  /// </summary>
  /// <param name="e">Event to subscribe to.</param>
  /// <param name="action">Action executed for each publication.</param>
  /// <returns>Disposable subscription handle.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  public static IDisposable Subscribe(this IEvent<Unit> e, Action action) {
    return e.Subscribe(Subscriber.Create(action));
  }

  /// <summary>
  /// Subscribes a subscriber that is invoked at most once.
  /// </summary>
  /// <typeparam name="T">Type of published event value.</typeparam>
  /// <param name="e">Event to subscribe to.</param>
  /// <param name="subscriber">Subscriber invoked for the first published value.</param>
  /// <exception cref="ArgumentNullException"><paramref name="subscriber"/> is <see langword="null"/>.</exception>
  public static void SubscribeOnce<T>(this IEvent<T> e, ISubscriber<T> subscriber) {
    ExceptionsHelper.ThrowIfNull(subscriber, nameof(subscriber));
    var handle = new DisposeHandle();
    handle.Register(e.Subscribe(new OnceSubscriber<T>(subscriber, handle)));
  }

  /// <summary>
  /// Subscribes an action that is invoked at most once.
  /// </summary>
  /// <typeparam name="T">Type of published event value.</typeparam>
  /// <param name="e">Event to subscribe to.</param>
  /// <param name="action">Action executed for the first published value.</param>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  public static void SubscribeOnce<T>(this IEvent<T> e, Action<T> action) {
    e.SubscribeOnce(Subscriber.Create(action));
  }

  /// <summary>
  /// Subscribes a parameterless action that is invoked at most once.
  /// </summary>
  /// <param name="e">Event to subscribe to.</param>
  /// <param name="action">Action executed for the first publication.</param>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  public static void SubscribeOnce(this IEvent<Unit> e, Action action) {
    e.SubscribeOnce(Subscriber.Create(action));
  }

}