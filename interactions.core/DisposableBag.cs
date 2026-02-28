using System.Runtime.ExceptionServices;

namespace Interactions.Core;

public sealed class DisposableBag : IDisposable {

  private readonly List<IDisposable> _source = [];
  private readonly object _lock = new();
  private int _disposed;

  public void Add(IDisposable disposable) {
    lock (_lock) {
      if (Volatile.Read(ref _disposed) != 0)
        throw new ObjectDisposedException(nameof(DisposableBag));

      ExceptionsHelper.ThrowIfNull(disposable, nameof(disposable));
      _source.Add(disposable);
    }
  }

  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;

    using ListPool<Exception>.ListHandle exceptions = ListPool<Exception>.Get();

    lock (_lock) {
      for (int i = _source.Count - 1; i >= 0; i--) {
        try {
          _source[i].Dispose();
        }
        catch (Exception e) {
          exceptions.Add(e);
        }
      }

      _source.Clear();
    }

    switch (exceptions.Count) {
      case > 1:
        throw new AggregateException(exceptions);
      case 1:
        ExceptionDispatchInfo.Capture(exceptions.Single()).Throw();
        break;
    }
  }

}