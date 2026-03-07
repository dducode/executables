using System.Diagnostics.Contracts;
using Interactions.Core.Internal;
using Interactions.Core.Subscribers;

namespace Interactions.Core;

public static class Subscriber {

  [Pure]
  public static ISubscriber<T> Create<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber<T>(action);
  }

  [Pure]
  public static ISubscriber<Unit> Create(Action action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber<Unit>(_ => action());
  }

}