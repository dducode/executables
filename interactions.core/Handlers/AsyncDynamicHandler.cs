using Interactions.Core.Providers;

namespace Interactions.Core.Handlers;

internal sealed class AsyncDynamicHandler<T1, T2>(IProvider<AsyncHandler<T1, T2>> provider) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    AsyncHandler<T1, T2> inner = provider.Get();
    return inner?.Execute(input, token) ?? throw new InvalidOperationException($"Cannot resolve handler by {provider.GetType().Name}");
  }

  protected override void DisposeCore() {
    provider.Dispose();
  }

}