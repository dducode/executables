namespace Interactions.Core.Subscribers;

internal sealed class ThreadPoolSubscriber<T>(ISubscriber<T> inner) : ISubscriber<T>, IExecutor<T, Unit> {

  public void Receive(T input) {
    ThreadPool.QueueUserWorkItem(_ => inner.Receive(input));
  }

  public IExecutor<T, Unit> GetExecutor() {
    return this;
  }

  Unit IExecutor<T, Unit>.Execute(T arg) {
    Receive(arg);
    return default;
  }

}