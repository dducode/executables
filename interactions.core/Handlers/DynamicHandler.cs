namespace Interactions.Core.Handlers;

internal sealed class DynamicHandler<T1, T2>(IProvider<Handler<T1, T2>> provider) : Handler<T1, T2> {

  protected override T2 ExecuteCore(T1 input) {
    Handler<T1, T2> inner = provider.Get();
    return inner != null ? inner.Execute(input) : throw new InvalidOperationException($"Cannot resolve handler by {provider.GetType().Name}");
  }

  protected override void DisposeCore() {
    provider.Dispose();
  }

}

internal sealed class AsyncDynamicHandler<T1, T2>(IProvider<AsyncHandler<T1, T2>> provider) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    AsyncHandler<T1, T2> inner = provider.Get();
    return inner?.Execute(input, token) ?? throw new InvalidOperationException($"Cannot resolve handler by {provider.GetType().Name}");
  }

  protected override void DisposeCore() {
    provider.Dispose();
  }

}