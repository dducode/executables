namespace Interactions.Core.Executables;

internal sealed class AsyncProxyExecutable<T1, T2>(IExecutable<T1, T2> inner) : IAsyncExecutable<T1, T2> {

  private readonly IExecutor<T1, T2> _inner = inner.GetExecutor();

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();
    return new ValueTask<T2>(_inner.Execute(input));
  }

}