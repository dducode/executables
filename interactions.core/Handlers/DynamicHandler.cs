using Interactions.Core.Providers;

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