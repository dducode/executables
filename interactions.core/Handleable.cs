namespace Interactions.Core;

public abstract class Handleable<T1, T2> {

  public abstract IDisposable Handle(Handler<T1, T2> handler);

}