namespace Interactions.Core;

public abstract partial class AsyncHandler<T1, T2> : IAsyncExecutable<T1, T2>, IDisposable {

  public bool Disposed => Volatile.Read(ref _disposed) != 0;

  private int _disposed;

  public ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    token.ThrowIfCancellationRequested();
    return HandleCore(input, token);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;
    DisposeCore();
  }

  protected abstract ValueTask<T2> HandleCore(T1 input, CancellationToken token = default);
  protected virtual void DisposeCore() { }

  public readonly struct Executor(AsyncHandler<T1, T2> handler) : IAsyncExecutor<T1, T2> {

    public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
      return handler.Handle(input, token);
    }

  }

}