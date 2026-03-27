using Interactions.Internal;

namespace Interactions.Handling;

public interface IAsyncHandleable<T1, T2, in THandler> where THandler : AsyncHandler<T1, T2> {

  IDisposable Handle(THandler handler);

}

public interface IAsyncHandleable<T1, T2> : IAsyncHandleable<T1, T2, AsyncHandler<T1, T2>>;

public abstract class 
  AsyncHandleable<T1, T2> : IAsyncHandleable<T1, T2> {

  protected AsyncHandler<T1, T2> Handler => Volatile.Read(ref _handlerNode)?.Handler;

  private readonly object _lock = new();
  private HandlerNode _handlerNode;

  public virtual IDisposable Handle(AsyncHandler<T1, T2> handler) {
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

  private class HandlerNode(AsyncHandleable<T1, T2> parent, AsyncHandler<T1, T2> handler) : IDisposable {

    internal AsyncHandler<T1, T2> Handler => handler;

    public void Dispose() {
      parent.RemoveNode(this);
    }

  }

}