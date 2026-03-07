using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Core.Lifecycle;
using Interactions.Handlers;

namespace Interactions.Commands;

public sealed class CommandContext : IDisposable, IUndoRedo {

  private const string ErrorMessage = "History corrupted: history size of context must be less or equal than minimal history size of one of the elements";

  public int HistoryCount => _history.Count;

  private readonly int _historyMaxSize;
  private readonly int _clearedElements;
  private readonly History<IUndoRedo> _history;
  private readonly DisposableBag _disposableBag;
  private readonly List<IUndoRedo> _clearedElementsBuffer = [];

  private bool _disposed;

  public CommandContext(int historyMaxSize = 256, int clearedElements = 64) {
    _historyMaxSize = historyMaxSize;
    if (clearedElements < 1 || historyMaxSize < clearedElements)
      throw new ArgumentOutOfRangeException(nameof(clearedElements));

    _clearedElements = clearedElements;
    _history = new History<IUndoRedo>(historyMaxSize);
    _disposableBag = new DisposableBag();
    _disposableBag.Add(_history);
  }

  public ICommand<TInput> CreateCommand<TInput, TChange>(ReversibleHandler<TInput, TChange> handler) {
    ThrowIfDisposed();
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    var command = new ContextualCommand<TInput, TChange>(this);
    _disposableBag.Add(command.Handle(handler));
    return command;
  }

  public bool Undo() {
    ThrowIfDisposed();

    if (!_history.Undo(out IUndoRedo value))
      return false;

    try {
      if (!value.Undo())
        throw new InvalidOperationException(ErrorMessage);
    }
    catch (Exception) {
      ClearHistory();
      throw;
    }

    return true;
  }

  public bool Redo() {
    ThrowIfDisposed();

    if (!_history.Redo(out IUndoRedo value))
      return false;

    try {
      if (!value.Redo())
        throw new InvalidOperationException(ErrorMessage);
    }
    catch (Exception) {
      ClearHistory();
      throw;
    }

    return true;
  }

  public void ClearHistory() {
    if (_disposed)
      return;

    _history.Clear(_clearedElementsBuffer);
    ClearCommandsHistory();
  }

  internal void Push(IUndoRedo item) {
    _history.Write(item);
    if (_history.Count < _historyMaxSize)
      return;

    _history.Clear(_clearedElements, _clearedElementsBuffer);
    ClearCommandsHistory();
  }

  private void ClearCommandsHistory() {
    foreach (IUndoRedo clearedItem in _clearedElementsBuffer)
      clearedItem.ClearHistory();
    _clearedElementsBuffer.Clear();
  }

  public void Dispose() {
    if (_disposed)
      return;

    _disposableBag.Dispose();
    _disposed = true;
  }

  private void ThrowIfDisposed() {
#if NET9_0_OR_GREATER
    ObjectDisposedException.ThrowIf(_disposed, this);
#else
    if (_disposed)
      throw new ObjectDisposedException(nameof(CommandContext));
#endif
  }

}