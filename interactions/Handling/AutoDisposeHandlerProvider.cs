using Interactions.Core.Handlers;

namespace Interactions.Handling;

public readonly ref struct AutoDisposeHandlerProvider<T1, T2>(Handler<T1, T2> handler, IDisposable handle) {

  public Handler<T1, T2> OnException<TEx>() where TEx : Exception {
    return new AutoDisposeHandler<T1, T2, TEx>(handler, handle);
  }

}