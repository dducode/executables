namespace Interactions.Core.Queries;

internal sealed class AsyncProxyQuery<T1, T2>(IQuery<T1, T2> inner) : IAsyncQuery<T1, T2> {

  public ValueTask<T2> Send(T1 input, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();
    return new ValueTask<T2>(inner.Send(input));
  }

}