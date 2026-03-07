using Interactions.Core.Handlers;

namespace Interactions.Core.Handleables;

public abstract class AsyncHandleable<T1, T2> {

  public abstract IDisposable Handle(AsyncHandler<T1, T2> handler);

}