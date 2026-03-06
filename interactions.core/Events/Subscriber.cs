using System.Diagnostics.Contracts;

namespace Interactions.Core.Events;

public interface ISubscriber<in T> {

  void Receive(T arg);

}

public static class Subscriber {

  [Pure]
  public static ISubscriber<T> FromMethod<T>(Action<T> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return new AnonymousSubscriber<T>(action);
  }

}