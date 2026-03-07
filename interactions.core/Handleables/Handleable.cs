using Interactions.Core.Handlers;

namespace Interactions.Core.Handleables;

public abstract class Handleable<T1, T2> {

  public abstract IDisposable Handle(Handler<T1, T2> handler);

}