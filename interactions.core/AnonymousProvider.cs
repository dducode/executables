namespace Interactions.Core;

internal sealed class AnonymousProvider<T>(Func<T> provide) : IProvider<T> {

  private int _disposed;

  public T Get() {
    return Volatile.Read(ref _disposed) == 0 ? provide() : throw new ObjectDisposedException(nameof(AnonymousProvider<T>));
  }

  public void Dispose() {
    Interlocked.Exchange(ref _disposed, 1);
  }

}