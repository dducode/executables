#if !NETFRAMEWORK
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly ref struct ExecutableReadOnlySpan<T1, T2>(IExecutable<T1, T2> executable, ReadOnlySpan<T1> source) {

  private readonly ReadOnlySpan<T1> _source = source;

  public ReadOnlySpanExecutor<T1, T2> GetEnumerator() {
    return new ReadOnlySpanExecutor<T1, T2>(executable, _source.GetEnumerator());
  }

}
#endif