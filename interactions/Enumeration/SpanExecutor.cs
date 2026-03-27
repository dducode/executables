#if !NETFRAMEWORK
namespace Interactions.Enumeration;

public ref struct SpanExecutor<T1, T2>(IQuery<T1, T2> query, Span<T1>.Enumerator source) {

  public T2 Current { get; private set; }
  private Span<T1>.Enumerator _source = source;

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = query.Send(_source.Current);
    return true;
  }

}
#endif