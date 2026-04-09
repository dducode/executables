#if !NETFRAMEWORK
namespace Executables.Enumeration;

public readonly struct AsyncExecutableEnumerable<T1, T2>(IAsyncExecutor<T1, T2> executor, IAsyncEnumerable<T1> source) : IAsyncEnumerable<T2> {

  public AsyncEnumerableExecutor<T1, T2> GetAsyncEnumerator(CancellationToken token = default) {
    return new AsyncEnumerableExecutor<T1, T2>(executor, source.GetAsyncEnumerator(token), token);
  }

  IAsyncEnumerator<T2> IAsyncEnumerable<T2>.GetAsyncEnumerator(CancellationToken token) {
    return GetAsyncEnumerator(token);
  }

}
#endif