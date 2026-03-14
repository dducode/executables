namespace Interactions.Core.Handlers;

internal sealed class AsyncProxyHandler<T1, T2>(Handler<T1, T2> inner) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    token.ThrowIfCancellationRequested();
    return new ValueTask<T2>(inner.Handle(input));
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}