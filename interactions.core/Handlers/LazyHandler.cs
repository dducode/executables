using Interactions.Core.Resolvers;

namespace Interactions.Core.Handlers;

internal sealed class LazyHandler<T1, T2>(IResolver<Handler<T1, T2>> resolver) : Handler<T1, T2> {

  private readonly Internal.Lazy<Handler<T1, T2>> _inner = new(resolver);

  protected override T2 ExecuteCore(T1 input) {
    return _inner.Value.Execute(input);
  }

  protected override void DisposeCore() {
    _inner.ValueOrDefault?.Dispose();
  }

}