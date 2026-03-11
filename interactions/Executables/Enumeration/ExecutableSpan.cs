#if !NETFRAMEWORK
using Interactions.Core;

namespace Interactions.Executables.Enumeration;

public readonly ref struct ExecutableSpan<T1, T2>(IExecutable<T1, T2> executable, Span<T1> source) {

  private readonly Span<T1> _source = source;

  public SpanExecutor<T1, T2> GetEnumerator() {
    return new SpanExecutor<T1, T2>(executable, _source.GetEnumerator());
  }

}

#endif