namespace Interactions.Core.Queries;

public class Query<T1, T2> : Handleable<T1, T2>, IExecutable<T1, T2> {

  private HandlerNode _handlerNode;
  private readonly object _lock = new();

  public virtual T2 Execute(T1 input) {
    HandlerNode node = Volatile.Read(ref _handlerNode);
    if (node == null)
      throw new MissingHandlerException("Cannot handle query");
    return node.HandleRequest(input);
  }

  public override IDisposable Handle(Handler<T1, T2> handler) {
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

  private class HandlerNode(Query<T1, T2> parent, Handler<T1, T2> handler) : IDisposable {

    public T2 HandleRequest(T1 request) {
      return handler.Execute(request);
    }

    public void Dispose() {
      parent.RemoveNode(this);
    }

  }

}