using Executables.Internal;

namespace Executables.Handling;

/// <summary>
/// Represents a source that can register a handler.
/// </summary>
/// <typeparam name="T1">Type of the handler input.</typeparam>
/// <typeparam name="T2">Type of the handler result.</typeparam>
/// <typeparam name="THandler">Type of the registered handler.</typeparam>
public interface IHandleable<T1, T2, in THandler> where THandler : Handler<T1, T2> {

  /// <summary>
  /// Registers a handler and returns a disposable registration handle.
  /// </summary>
  /// <param name="handler">Handler to register.</param>
  /// <returns>Handle that unregisters the handler when disposed.</returns>
  IDisposable Handle(THandler handler);

}

/// <summary>
/// Represents a source that can register a handler.
/// </summary>
/// <typeparam name="T1">Type of the handler input.</typeparam>
/// <typeparam name="T2">Type of the handler result.</typeparam>
public interface IHandleable<T1, T2> : IHandleable<T1, T2, Handler<T1, T2>>;

/// <summary>
/// Base class for handleables that support a single registered handler.
/// </summary>
/// <typeparam name="T1">Type of the handler input.</typeparam>
/// <typeparam name="T2">Type of the handler result.</typeparam>
public abstract class Handleable<T1, T2> : IHandleable<T1, T2> {

  /// <summary>
  /// Currently registered handler, or <see langword="null"/> when no handler is registered.
  /// </summary>
  protected Handler<T1, T2> Handler => Volatile.Read(ref _handlerNode)?.Handler;

  private readonly object _lock = new();
  private HandlerNode _handlerNode;

  /// <summary>
  /// Registers a handler.
  /// </summary>
  /// <param name="handler">Handler to register.</param>
  /// <returns>Handle that unregisters the handler when disposed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
  /// <exception cref="InvalidOperationException">A handler is already registered.</exception>
  public virtual IDisposable Handle(Handler<T1, T2> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));

    lock (_lock) {
      if (_handlerNode != null)
        throw new InvalidOperationException("Already has handler");
      var handle = new HandlerNode(this, handler);
      handler.RegisterHandle(handle);
      return _handlerNode = handle;
    }
  }

  private void RemoveNode(HandlerNode node) {
    Interlocked.CompareExchange(ref _handlerNode, null, node);
    node.Handler.UnregisterHandle(node);
  }

  private class HandlerNode(Handleable<T1, T2> parent, Handler<T1, T2> handler) : IDisposable {

    internal Handler<T1, T2> Handler => handler;

    public void Dispose() {
      parent.RemoveNode(this);
    }

  }

}