using System.Runtime.CompilerServices;

namespace Interactions.Core;

public abstract partial class AsyncHandler<T1, T2> : IDisposable {

  public bool Disposed => Volatile.Read(ref _disposed) != 0;

  private int _disposed;

  public abstract ValueTask<T2> Handle(T1 input, CancellationToken token = default);

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;
    DisposeCore();
  }

  protected virtual void DisposeCore() { }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected void ThrowIfDisposed(string objectName) {
    if (Disposed)
      throw new HandlerDisposedException(objectName);
  }

}