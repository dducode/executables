namespace Interactions.Core.Queries;

public interface IAsyncQuery<in T1, T2> {

  ValueTask<T2> Send(T1 input, CancellationToken token = default);

}

public class AsyncQuery<T1, T2> : AsyncHandleable<T1, T2>, IAsyncQuery<T1, T2> {

  private HandlerNode _handlerNode;
  private readonly object _lock = new();

  public virtual ValueTask<T2> Send(T1 input, CancellationToken token = default) {
    HandlerNode node = Volatile.Read(ref _handlerNode);
    if (node == null)
      throw new MissingHandlerException("Cannot handle query");
    return node.HandleRequest(input, token);
  }

  public override IDisposable Handle(AsyncHandler<T1, T2> handler) {
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

  private class HandlerNode(AsyncQuery<T1, T2> parent, AsyncHandler<T1, T2> handler) : IDisposable {

    public ValueTask<T2> HandleRequest(T1 input, CancellationToken token) {
      return handler.Handle(input, token);
    }

    public void Dispose() {
      parent.RemoveNode(this);
      handler.Dispose();
    }

  }

}