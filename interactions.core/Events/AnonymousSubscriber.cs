namespace Interactions.Core.Events;

internal sealed class AnonymousSubscriber<T>(Action<T> action) : ISubscriber<T> {

  public Unit Execute(T arg) {
    action(arg);
    return default;
  }

}