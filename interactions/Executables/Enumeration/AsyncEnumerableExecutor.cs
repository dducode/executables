#if !NETFRAMEWORK
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public class AsyncEnumerableExecutor<T1, T2>(IAsyncExecutable<T1, T2> executable, IAsyncEnumerator<T1> source, CancellationToken token) : IAsyncEnumerator<T2> {

  public T2 Current { get; private set; }

  public async ValueTask<bool> MoveNextAsync() {
    if (!await source.MoveNextAsync())
      return false;
    Current = await executable.Execute(source.Current, token);
    return true;
  }

  public ValueTask DisposeAsync() {
    return source.DisposeAsync();
  }

}
#endif