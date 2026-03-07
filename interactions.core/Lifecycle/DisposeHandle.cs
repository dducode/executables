using Interactions.Core.Internal;

namespace Interactions.Core.Lifecycle;

public sealed class DisposeHandle : IDisposable {

  private IDisposable _disposable;
  private int _disposed;

  public void Register(IDisposable disposable) {
    if (Volatile.Read(ref _disposed) != 0)
      throw new ObjectDisposedException(nameof(DisposeHandle));

    ExceptionsHelper.ThrowIfNull(disposable, nameof(disposable));
    if (Interlocked.CompareExchange(ref _disposable, disposable, null) != null)
      throw new InvalidOperationException("Disposable already registered");
  }

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