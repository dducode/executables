using System.Diagnostics.Contracts;

namespace Interactions.Core.Events;

public static class SubscriberExtensions {

  [Pure]
  public static ISubscriber<T> Once<T>(this ISubscriber<T> subscriber, IDisposable handle) {
    ExceptionsHelper.ThrowIfNullReference(subscriber);
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new OnceSubscriber<T>(subscriber, handle);
  }

  [Pure]
  public static ISubscriber<T> OnThreadPool<T>(this ISubscriber<T> subscriber) {
    ExceptionsHelper.ThrowIfNullReference(subscriber);
    return new ThreadPoolSubscriber<T>(subscriber);
  }

}