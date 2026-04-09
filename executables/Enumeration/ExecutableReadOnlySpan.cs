#if !NETFRAMEWORK
namespace Executables.Enumeration;

public readonly ref struct ExecutableReadOnlySpan<T1, T2>(IExecutor<T1, T2> executor, ReadOnlySpan<T1> source) {

  private readonly ReadOnlySpan<T1> _source = source;

  public ReadOnlySpanExecutor<T1, T2> GetEnumerator() {
    return new ReadOnlySpanExecutor<T1, T2>(executor, _source.GetEnumerator());
  }

}
#endif