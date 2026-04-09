namespace Executables.Core.Executables;

internal sealed class AsyncExecutableQuery<T1, T2>(IAsyncExecutable<T1, T2> inner) : IAsyncQuery<T1, T2>, IAsyncExecutor<T1, T2> {

  private readonly IAsyncExecutor<T1, T2> _inner = inner.GetExecutor();

  public ValueTask<T2> Send(T1 input, CancellationToken token = default) {
    return _inner.Execute(input, token);
  }

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    return Send(input, token);
  }

}