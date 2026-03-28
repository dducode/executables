using Interactions.Core.Handlers;

namespace Interactions.Handling;

public readonly ref struct AsyncAutoDetachableHandlerProvider<T1, T2>(AsyncHandler<T1, T2> handler) {

  public AsyncHandler<T1, T2> OnException<TEx>() where TEx : Exception {
    return new AsyncAutoDetachableHandler<T1, T2, TEx>(handler);
  }

}