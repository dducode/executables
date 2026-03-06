namespace Interactions.Core.Handlers;

internal sealed class AsyncLazyHandler<T1, T2>(IResolver<AsyncHandler<T1, T2>> resolver) : AsyncHandler<T1, T2> {

  private readonly Lazy<AsyncHandler<T1, T2>> _inner = new(resolver);

  protected override ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    return _inner.Value.Handle(input, token);
  }

  protected override void DisposeCore() {
    _inner.ValueOrDefault?.Dispose();
  }

}