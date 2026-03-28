using Interactions.Core.Handlers;

namespace Interactions.Handling;

public readonly ref struct AutoDetachableHandlerProvider<T1, T2>(Handler<T1, T2> handler) {

  public Handler<T1, T2> OnException<TEx>() where TEx : Exception {
    return new AutoDetachableHandler<T1, T2, TEx>(handler);
  }

}