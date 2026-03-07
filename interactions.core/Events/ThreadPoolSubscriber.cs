namespace Interactions.Core.Events;

internal sealed class ThreadPoolSubscriber<T>(ISubscriber<T> inner) : ISubscriber<T> {

  public Unit Execute(T arg) {
    ThreadPool.QueueUserWorkItem(_ => inner.Execute(arg));
    return default;
  }

}