using Interactions.Internal;

namespace Interactions.Handling;

public abstract class DisposableHandler : IDisposable {

  public bool Disposed => Volatile.Read(ref disposeState) != 0;

  private protected int disposeState;
  private readonly List<IDisposable> _handles = [];
  private readonly object _lock = new();

  public void Dispose() {
    if (Interlocked.Exchange(ref disposeState, 1) != 0)
      return;

    DisposeHandles();
    DisposeCore();
  }

  protected virtual void DisposeCore() { }

  protected void DisposeHandles() {
    List<IDisposable> handles = Pool<List<IDisposable>>.Get();
    using var handlesHandle = new ListHandle<IDisposable>(handles);

    lock (_lock) {
      handles.AddRange(_handles);
      _handles.Clear();
    }

    foreach (IDisposable handle in handles)
      handle.Dispose();
  }

  internal void RegisterHandle(IDisposable handle) {
    lock (_lock) {
      ThrowIfDisposed();
      _handles.Add(handle);
    }
  }

  internal void UnregisterHandle(IDisposable handle) {
    lock (_lock)
      _handles.Remove(handle);
  }

  private protected void ThrowIfDisposed() {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
  }

}