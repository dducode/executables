namespace Interactions.Core.Subscribers;

internal sealed class OnceSubscriber<T>(ISubscriber<T> inner, IDisposable handle) : ISubscriber<T> {

  public Unit Execute(T arg) {
    try {
      inner.Execute(arg);
      return default;
    }
    finally {
      handle.Dispose();
    }
  }

}