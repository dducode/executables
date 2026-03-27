using System.Diagnostics.Contracts;
using Interactions.Core.Subscribers;
using Interactions.Internal;

namespace Interactions.Subscribers;

public static class Subscriber {

  [Pure]
  public static ISubscriber<T> Create<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber<T>(action);
  }

  [Pure]
  public static ISubscriber<Unit> Create(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber(action);
  }

}