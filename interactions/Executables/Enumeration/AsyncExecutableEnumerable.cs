#if !NETFRAMEWORK
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly struct AsyncExecutableEnumerable<T1, T2>(IAsyncExecutable<T1, T2> executable, IAsyncEnumerable<T1> source) : IAsyncEnumerable<T2> {

  public AsyncEnumerableExecutor<T1, T2> GetAsyncEnumerator(CancellationToken token = default) {
    return new AsyncEnumerableExecutor<T1, T2>(executable, source.GetAsyncEnumerator(token), token);
  }

  IAsyncEnumerator<T2> IAsyncEnumerable<T2>.GetAsyncEnumerator(CancellationToken token) {
    return GetAsyncEnumerator(token);
  }

}
#endif