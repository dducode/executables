using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Base class for handlers with disposable lifetime management.
/// </summary>
public abstract class DisposableHandler : IDisposable {

  /// <summary>
  /// Gets whether the handler has been disposed.
  /// </summary>
  public bool Disposed => Volatile.Read(ref _disposeState) != 0;

  private readonly List<IDisposable> _handles = [];
  private readonly object _lock = new();
  private int _disposeState;

  /// <summary>
  /// Disposes the handler and all registered handles.
  /// </summary>
  public void Dispose() {
    if (Interlocked.Exchange(ref _disposeState, 1) != 0)
      return;

    DisposeHandles();
    DisposeCore();
  }

  /// <summary>
  /// Allows derived classes to release their own resources during disposal.
  /// </summary>
  protected virtual void DisposeCore() { }

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

  private void DisposeHandles() {
    List<IDisposable> handles = Pool<List<IDisposable>>.Get();
    using var handlesHandle = new ListHandle<IDisposable>(handles);

    lock (_lock) {
      handles.AddRange(_handles);
      _handles.Clear();
    }

    foreach (IDisposable handle in handles)
      handle.Dispose();
  }

  private protected void ThrowIfDisposed() {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
  }

}