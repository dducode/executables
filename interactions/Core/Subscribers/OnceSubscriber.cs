using Interactions.Subscribers;

namespace Interactions.Core.Subscribers;

internal sealed class OnceSubscriber<T>(ISubscriber<T> inner, IDisposable handle) : ISubscriber<T>, IExecutor<T, Unit> {

  public void Receive(T input) {
    try {
      inner.Receive(input);
    }
    finally {
      handle.Dispose();
    }
  }

  IExecutor<T, Unit> IExecutable<T, Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<T, Unit>.Execute(T arg) {
    Receive(arg);
    return default;
  }

}