#if !NETFRAMEWORK
namespace Executables.Enumeration;

public ref struct ReadOnlySpanExecutor<T1, T2>(IExecutor<T1, T2> executor, ReadOnlySpan<T1>.Enumerator source) {

  public T2 Current { get; private set; }
  private ReadOnlySpan<T1>.Enumerator _source = source;

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = executor.Execute(_source.Current);
    return true;
  }

}
#endif