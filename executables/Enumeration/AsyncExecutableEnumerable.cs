#if !NETFRAMEWORK
namespace Executables.Enumeration;

public readonly struct AsyncExecutableEnumerable<T1, T2>(IAsyncQuery<T1, T2> query, IAsyncEnumerable<T1> source) : IAsyncEnumerable<T2> {

  public AsyncEnumerableExecutor<T1, T2> GetAsyncEnumerator(CancellationToken token = default) {
    return new AsyncEnumerableExecutor<T1, T2>(query, source.GetAsyncEnumerator(token), token);
  }

  IAsyncEnumerator<T2> IAsyncEnumerable<T2>.GetAsyncEnumerator(CancellationToken token) {
    return GetAsyncEnumerator(token);
  }

}
#endif