#if !NETFRAMEWORK
namespace Interactions.Enumeration;

public readonly ref struct ExecutableReadOnlySpan<T1, T2>(IQuery<T1, T2> query, ReadOnlySpan<T1> source) {

  private readonly ReadOnlySpan<T1> _source = source;

  public ReadOnlySpanExecutor<T1, T2> GetEnumerator() {
    return new ReadOnlySpanExecutor<T1, T2>(query, _source.GetEnumerator());
  }

}
#endif