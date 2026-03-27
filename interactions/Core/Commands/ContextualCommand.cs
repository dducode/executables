using System.Diagnostics;
using Interactions.Commands;
using Interactions.Handling;

namespace Interactions.Core.Commands;

internal sealed class ContextualCommand<TInput, TChange> : ICommand<TInput>, IExecutor<TInput, bool>, IUndoRedo, IDisposable {

  private readonly CommandContext _context;
  private readonly History<TChange> _history;

  private ReversibleHandler<TInput, TChange> _handler;
  private bool _disposed;

  internal ContextualCommand(CommandContext context) {
    _context = context;
    _history = new History<TChange>();
  }

  public bool Execute(TInput input) {
    ThrowIfDisposed();

    Debug.Assert(_handler != null);
    _history.Write(_handler.Handle(input));
    _context.Push(this);
    return true;
  }

  public IExecutor<TInput, bool> GetExecutor() {
    return this;
  }

  public bool Undo() {
    if (!_history.Undo(out TChange value))
      return false;

    Debug.Assert(_handler != null);
    _handler.Undo(value);
    return true;
  }

  public bool Redo() {
    if (!_history.Redo(out TChange change))
      return false;

    Debug.Assert(_handler != null);
    _handler.Redo(change);
    return true;
  }

  public void ClearHistory() {
    if (_disposed)
      return;

    _history.Clear();
  }

  public IDisposable Handle(ReversibleHandler<TInput, TChange> handler) {
    _handler = handler;
    return this;
  }

  public void Dispose() {
    if (_disposed)
      return;

    _history.Dispose();
    _handler?.Dispose();
    _handler = null;
    _disposed = true;
  }

  private void ThrowIfDisposed() {
#if NET9_0_OR_GREATER
    ObjectDisposedException.ThrowIf(_disposed, this);
#else
    if (_disposed)
      throw new ObjectDisposedException(nameof(ContextualCommand<TInput, TChange>));
#endif
  }

}