#if !NETFRAMEWORK
namespace Interactions.Core;

public abstract partial class AsyncHandler<T1, T2> : IAsyncDisposable {

  public ValueTask DisposeAsync() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return default;
    return AsyncDisposeCore();
  }

  protected virtual ValueTask AsyncDisposeCore() {
    return default;
  }

}
#endif