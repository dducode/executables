namespace Executables.Core.Executables;

internal sealed class AsyncOrElseExecutable<T1, T2>(IAsyncExecutable<T1, Optional<T2>> inner, IAsyncExecutable<T1, T2> other)
  : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, Optional<T2>> _inner = inner.GetExecutor();
  private readonly IAsyncExecutor<T1, T2> _other = other.GetExecutor();

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  async ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    Optional<T2> result = await _inner.Execute(input, token);
    return result.HasValue ? result.Value : await _other.Execute(input, token);
  }

}