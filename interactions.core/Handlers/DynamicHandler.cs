namespace Interactions.Core.Handlers;

internal sealed class DynamicHandler<T1, T2>(IProvider<Handler<T1, T2>> provider) : Handler<T1, T2> {

  public override T2 Handle(T1 input) {
    ThrowIfDisposed(nameof(DynamicHandler<T1, T2>));
    using Handler<T1, T2> handler = provider.Get();
    return handler != null ? handler.Handle(input) : throw new InvalidOperationException($"Cannot resolve handler by {provider.GetType().Name}");
  }

  protected override void DisposeCore() {
    provider.Dispose();
  }

}

internal sealed class AsyncDynamicHandler<T1, T2>(IProvider<AsyncHandler<T1, T2>> provider) : AsyncHandler<T1, T2> {

  public override async ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncDynamicHandler<T1, T2>));
    using AsyncHandler<T1, T2> handler = provider.Get();
    return handler != null ? await handler.Handle(input, token) : throw new InvalidOperationException($"Cannot resolve handler by {provider.GetType().Name}");
  }

  protected override void DisposeCore() {
    provider.Dispose();
  }

}