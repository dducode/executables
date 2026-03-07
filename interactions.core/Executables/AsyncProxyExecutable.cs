namespace Interactions.Core.Executables;

internal sealed class AsyncProxyExecutable<T1, T2>(IExecutable<T1, T2> inner) : IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();
    return new ValueTask<T2>(inner.Execute(input));
  }

}