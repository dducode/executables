using System.Runtime.ExceptionServices;
using Executables.Internal;

namespace Executables.Lifecycle;

/// <summary>
/// Stores multiple disposable objects and disposes them together.
/// </summary>
public sealed class DisposableBag : IDisposable {

  private readonly List<IDisposable> _source = [];
  private readonly object _lock = new();
  private int _disposed;

  /// <summary>
  /// Adds a disposable object to the bag.
  /// </summary>
  /// <param name="disposable">Disposable object to add.</param>
  /// <exception cref="ArgumentNullException"><paramref name="disposable"/> is <see langword="null"/>.</exception>
  /// <exception cref="ObjectDisposedException">The bag has already been disposed.</exception>
  public void Add(IDisposable disposable) {
    lock (_lock) {
      if (Volatile.Read(ref _disposed) != 0)
        throw new ObjectDisposedException(nameof(DisposableBag));

      ExceptionsHelper.ThrowIfNull(disposable, nameof(disposable));
      _source.Add(disposable);
    }
  }

  /// <summary>
  /// Disposes all registered objects in reverse registration order.
  /// </summary>
  /// <exception cref="AggregateException">More than one registered disposable throws during disposal.</exception>
  public void Dispose() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return;

    List<Exception> exceptions = Pool<List<Exception>>.Get();
    using var handle = new ListHandle<Exception>(exceptions);

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
        ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
        break;
    }
  }

}