using Interactions.Core.Handlers;

namespace Interactions.Handling;

public readonly ref struct AsyncAutoDisposeHandlerProvider<T1, T2>(AsyncHandler<T1, T2> handler, IDisposable handle) {

  public AsyncHandler<T1, T2> OnException<TEx>() where TEx : Exception {
    return new AsyncAutoDisposeHandler<T1, T2, TEx>(handler, handle);
  }

}