using Interactions.Core.Executables;

namespace Interactions.Core.Handlers;

public abstract partial class AsyncHandler<T1, T2> : IAsyncExecutable<T1, T2>, IDisposable {

  public bool Disposed => Volatile.Read(ref _disposed) != 0;

  private int _disposed;

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    token.ThrowIfCancellationRequested();
    return ExecuteCore(input, token);
  }

  protected abstract ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default);

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;
    DisposeCore();
  }

  protected virtual void DisposeCore() { }

}