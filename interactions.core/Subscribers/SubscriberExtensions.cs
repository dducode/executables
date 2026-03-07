using System.Diagnostics.Contracts;
using Interactions.Core.Events;

namespace Interactions.Core.Subscribers;

public static class SubscriberExtensions {

  [Pure]
  public static ISubscriber<T> Once<T>(this ISubscriber<T> subscriber, IDisposable handle) {
    subscriber.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new OnceSubscriber<T>(subscriber, handle);
  }

  [Pure]
  public static ISubscriber<T> OnThreadPool<T>(this ISubscriber<T> subscriber) {
    subscriber.ThrowIfNullReference();
    return new ThreadPoolSubscriber<T>(subscriber);
  }

}