using System.Diagnostics.Contracts;
using Executables.Core.Subscribers;
using Executables.Internal;

namespace Executables.Subscribers;

public static class SubscriberExtensions {

  [Pure]
  public static ISubscriber<T> Once<T>(this ISubscriber<T> subscriber, IDisposable handle) {
    subscriber.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new OnceSubscriber<T>(subscriber, handle);
  }

}