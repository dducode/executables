using Interactions.Core.Handlers;
using Interactions.Internal;

namespace Interactions.Handling;

public readonly ref struct AutoDisposeHandlerProvider<T1, T2>(Handler<T1, T2> handler) {

  public Handler<T1, T2> OfType<TEx>(IDisposable handle) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AutoDisposeHandler<T1, T2, TEx>(handler, handle);
  }

}