namespace Interactions.Core.Commands;

public interface ICommand<in T> {

  bool Execute(T input);

}

public class Command<T> : Handleable<T, Unit>, ICommand<T> {

  private HandlerNode _handlerNode;
  private readonly object _lock = new();

  public virtual bool Execute(T input) {
    return Volatile.Read(ref _handlerNode)?.ExecuteCommand(input) ?? false;
  }

  public override IDisposable Handle(Handler<T, Unit> handler) {
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

  private class HandlerNode(Command<T> parent, Handler<T, Unit> handler) : IDisposable {

    public bool ExecuteCommand(T input) {
      handler.Handle(input);
      return true;
    }

    public void Dispose() {
      parent.RemoveNode(this);
    }

  }

}