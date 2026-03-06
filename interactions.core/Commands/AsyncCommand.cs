namespace Interactions.Core.Commands;

public interface IAsyncCommand<in T> {

  ValueTask<bool> Execute(T input, CancellationToken token = default);

}

public class AsyncCommand<T> : AsyncHandleable<T, Unit>, IAsyncCommand<T> {

  private HandlerNode _handlerNode;
  private readonly object _lock = new();

  public virtual ValueTask<bool> Execute(T input, CancellationToken token = default) {
    if (token.IsCancellationRequested)
      return new ValueTask<bool>(false);
    HandlerNode node = Volatile.Read(ref _handlerNode);
    return node?.ExecuteCommand(input, token) ?? new ValueTask<bool>(false);
  }

  public override IDisposable Handle(AsyncHandler<T, Unit> handler) {
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

  private class HandlerNode(AsyncCommand<T> parent, AsyncHandler<T, Unit> handler) : IDisposable {

    public async ValueTask<bool> ExecuteCommand(T input, CancellationToken token) {
      try {
        await handler.Execute(input, token);
        return true;
      }
      catch (OperationCanceledException) {
        return false;
      }
    }

    public void Dispose() {
      parent.RemoveNode(this);
    }

  }

}