using System.Diagnostics;
using Interactions.Core;
using Interactions.Handlers;

namespace Interactions.Commands;

/// <summary>
/// Not thread-safe command. Intended for single-threaded (UI) usage.
/// </summary>
/// <typeparam name="TInput">Input type</typeparam>
/// <typeparam name="TChange">Previous state for revert</typeparam>
public sealed class ReversibleCommand<TInput, TChange> : ICommand<TInput>, IUndoRedo {

  private readonly History<TChange> _history;
  private readonly int _historyMaxSize;
  private readonly int _clearedElements;

  private HandlerNode _handlerNode;

  public ReversibleCommand(int historyMaxSize = 256, int clearedElements = 64) {
    _historyMaxSize = historyMaxSize;
    if (clearedElements < 1 || historyMaxSize < clearedElements)
      throw new ArgumentOutOfRangeException(nameof(clearedElements));

    _clearedElements = clearedElements;
    _history = new History<TChange>(historyMaxSize);
  }

  public bool Execute(TInput input) {
    if (_handlerNode == null)
      return false;

    _history.Write(_handlerNode.ExecuteCommand(input));
    if (_history.Count == _historyMaxSize)
      _history.Clear(_clearedElements);
    return true;
  }

  public bool Undo() {
    if (!_history.Undo(out TChange state))
      return false;

    Debug.Assert(_handlerNode != null);
    _handlerNode.Undo(state);
    return true;
  }

  public bool Redo() {
    if (!_history.Redo(out TChange state))
      return false;

    Debug.Assert(_handlerNode != null);
    _handlerNode.Redo(state);
    return true;
  }

  public void ClearHistory() {
    _history.Clear();
  }

  public IDisposable Handle(ReversibleHandler<TInput, TChange> handler) {
    if (_handlerNode != null)
      throw new InvalidOperationException("Already has handler");
    return _handlerNode = new HandlerNode(this, handler);
  }

  private void RemoveNode(HandlerNode node) {
    if (_handlerNode != node)
      return;
    _handlerNode = null;
    ClearHistory();
  }

  private class HandlerNode(ReversibleCommand<TInput, TChange> parent, ReversibleHandler<TInput, TChange> handler) : IDisposable {

    public TChange ExecuteCommand(TInput input) {
      return handler.Execute(input);
    }

    public void Undo(TChange state) {
      handler.Undo(state);
    }

    public void Redo(TChange state) {
      handler.Redo(state);
    }

    public void Dispose() {
      parent.RemoveNode(this);
      handler.Dispose();
    }

  }

}