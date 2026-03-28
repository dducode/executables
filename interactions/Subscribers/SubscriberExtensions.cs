using System.Diagnostics.Contracts;
using Interactions.Core.Subscribers;
using Interactions.Internal;

namespace Interactions.Subscribers;

public static class SubscriberExtensions {

  [Pure]
  public static ISubscriber<T> Once<T>(this ISubscriber<T> subscriber, IDisposable handle) {
    subscriber.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new OnceSubscriber<T>(subscriber, handle);
  }

}