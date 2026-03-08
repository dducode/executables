using Interactions.Core.Internal;

namespace Interactions.Core;

public interface IHandleable<T1, T2, in THandler> where THandler : Handler<T1, T2> {

  IDisposable Handle(THandler handler);

}

public interface IHandleable<T1, T2> : IHandleable<T1, T2, Handler<T1, T2>>;

public abstract class Handleable<T1, T2> : IHandleable<T1, T2> {

  protected Handler<T1, T2> Handler => Volatile.Read(ref _handlerNode)?.Handler;

  private readonly object _lock = new();
  private HandlerNode _handlerNode;

  public virtual IDisposable Handle(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));

    lock (_lock) {
      if (_handlerNode != null)
        throw new InvalidOperationException("Already has handler");
      return _handlerNode = new HandlerNode(this, handler);
    }
  }

  private void RemoveNode(HandlerNode node) {
    Interlocked.CompareExchange(ref _handlerNode, null, node);
  }

  private class HandlerNode(Handleable<T1, T2> parent, Handler<T1, T2> handler) : IDisposable {

    internal Handler<T1, T2> Handler => handler;

    public void Dispose() {
      parent.RemoveNode(this);
    }

  }

}