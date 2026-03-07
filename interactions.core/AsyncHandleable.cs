namespace Interactions.Core;

public abstract class AsyncHandleable<T1, T2> {

  public abstract IDisposable Handle(AsyncHandler<T1, T2> handler);

}