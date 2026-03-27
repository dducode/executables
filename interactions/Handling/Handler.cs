namespace Interactions.Handling;

public abstract class Handler<T1, T2> : IExecutable<T1, T2>, IDisposable {

  public bool Disposed => Volatile.Read(ref _disposed) != 0;

  private int _disposed;

  public T2 Handle(T1 input) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    return HandleCore(input);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;
    DisposeCore();
  }

  protected abstract T2 HandleCore(T1 input);
  protected virtual void DisposeCore() { }

  public readonly struct Executor(Handler<T1, T2> handler) : IExecutor<T1, T2> {

    public T2 Execute(T1 input) {
      return handler.Handle(input);
    }

  }

}