#if !NETFRAMEWORK
namespace Interactions.Handling;

public abstract partial class AsyncHandler<T1, T2> : IAsyncDisposable {

  public ValueTask DisposeAsync() {
    if (Interlocked.Exchange(ref _disposed, 1) != 0)
      return new ValueTask(Task.CompletedTask);
    return AsyncDisposeCore();
  }

  protected virtual ValueTask AsyncDisposeCore() {
    return new ValueTask(Task.CompletedTask);
  }

}
#endif