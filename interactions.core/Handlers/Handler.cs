using Interactions.Core.Executables;

namespace Interactions.Core.Handlers;

public abstract partial class Handler<T1, T2> : IExecutable<T1, T2>, IDisposable {

  public bool Disposed => Volatile.Read(ref _disposed) != 0;

  private int _disposed;

  public T2 Execute(T1 input) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    return ExecuteCore(input);
  }

  protected abstract T2 ExecuteCore(T1 input);

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;
    DisposeCore();
  }

  protected virtual void DisposeCore() { }

}