namespace Interactions.Core.Events;

internal sealed class OnceSubscriber<T>(ISubscriber<T> inner, IDisposable handle) : ISubscriber<T> {

  public void Receive(T arg) {
    try {
      inner.Receive(arg);
    }
    finally {
      handle.Dispose();
    }
  }

}