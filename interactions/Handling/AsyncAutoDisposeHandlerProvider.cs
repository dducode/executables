using Interactions.Core.Handlers;
using Interactions.Internal;

namespace Interactions.Handling;

public readonly ref struct AsyncAutoDisposeHandlerProvider<T1, T2>(AsyncHandler<T1, T2> handler) {

  public AsyncHandler<T1, T2> OfType<TEx>(IDisposable handle) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(handle, nameof(handle));
    return new AsyncAutoDisposeHandler<T1, T2, TEx>(handler, handle);
  }

}