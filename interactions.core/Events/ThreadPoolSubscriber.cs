namespace Interactions.Core.Events;

internal sealed class ThreadPoolSubscriber<T>(ISubscriber<T> inner) : ISubscriber<T> {

  public void Receive(T arg) {
    ThreadPool.QueueUserWorkItem(_ => inner.Receive(arg));
  }

}