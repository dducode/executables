#if !NETFRAMEWORK
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public ref struct ReadOnlySpanExecutor<T1, T2>(IQuery<T1, T2> query, ReadOnlySpan<T1>.Enumerator source) {

  private ReadOnlySpan<T1>.Enumerator _source = source;

  public T2 Current { get; private set; }

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = query.Send(_source.Current);
    return true;
  }

}
#endif