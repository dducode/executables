namespace Interactions.Core;

public abstract partial class Handler<T1, T2> : IDisposable {

  public bool Disposed => Volatile.Read(ref _disposed) != 0;

  private int _disposed;

  public T2 Handle(T1 input) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    return HandleCore(input);
  }

  protected abstract T2 HandleCore(T1 input);

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;
    DisposeCore();
  }

  protected virtual void DisposeCore() { }

}