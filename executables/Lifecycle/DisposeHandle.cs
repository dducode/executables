using Executables.Internal;

namespace Executables.Lifecycle;

/// <summary>
/// Holds a single disposable object that can be registered later and disposed once.
/// </summary>
public sealed class DisposeHandle : IDisposable {

  private IDisposable _disposable;
  private int _disposed;

  /// <summary>
  /// Registers the disposable object managed by this handle.
  /// </summary>
  /// <param name="disposable">Disposable object to register.</param>
  /// <exception cref="ArgumentNullException"><paramref name="disposable"/> is <see langword="null"/>.</exception>
  /// <exception cref="ObjectDisposedException">The handle has already been disposed.</exception>
  /// <exception cref="InvalidOperationException">A disposable object has already been registered.</exception>
  public void Register(IDisposable disposable) {
    if (Volatile.Read(ref _disposed) != 0)
      throw new ObjectDisposedException(nameof(DisposeHandle));

    ExceptionsHelper.ThrowIfNull(disposable, nameof(disposable));
    if (Interlocked.CompareExchange(ref _disposable, disposable, null) != null)
      throw new InvalidOperationException("Disposable already registered");
  }

  /// <summary>
  /// Disposes the registered disposable object, if one was registered.
  /// </summary>
  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;

    try {
      _disposable?.Dispose();
    }
    finally {
      _disposable = null;
    }
  }

}