#if !NETFRAMEWORK
namespace Executables.Enumeration;

public readonly ref struct ExecutableSpan<T1, T2>(IExecutor<T1, T2> executor, Span<T1> source) {

  private readonly Span<T1> _source = source;

  public SpanExecutor<T1, T2> GetEnumerator() {
    return new SpanExecutor<T1, T2>(executor, _source.GetEnumerator());
  }

}

#endif