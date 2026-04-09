#if !NETFRAMEWORK
namespace Executables.Enumeration;

public class AsyncEnumerableExecutor<T1, T2>(IAsyncExecutor<T1, T2> executor, IAsyncEnumerator<T1> source, CancellationToken token) : IAsyncEnumerator<T2> {

  public T2 Current { get; private set; }

  public async ValueTask<bool> MoveNextAsync() {
    if (!await source.MoveNextAsync())
      return false;
    Current = await executor.Execute(source.Current, token);
    return true;
  }

  public ValueTask DisposeAsync() {
    return source.DisposeAsync();
  }

}
#endif