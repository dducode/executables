#if !NETFRAMEWORK
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public ref struct ReadOnlySpanExecutor<T1, T2>(IExecutable<T1, T2> executable, ReadOnlySpan<T1>.Enumerator source) {

  private ReadOnlySpan<T1>.Enumerator _source = source;

  public T2 Current { get; private set; }

  public bool MoveNext() {
    if (!_source.MoveNext())
      return false;
    Current = executable.Execute(_source.Current);
    return true;
  }

}
#endif