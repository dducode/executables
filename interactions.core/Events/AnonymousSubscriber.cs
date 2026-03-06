namespace Interactions.Core.Events;

internal sealed class AnonymousSubscriber<T>(Action<T> action) : ISubscriber<T> {

  public void Receive(T arg) {
    action(arg);
  }

}