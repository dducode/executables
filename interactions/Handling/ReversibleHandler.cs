namespace Interactions.Handling;

public abstract class ReversibleHandler<TInput, TChange> : Handler<TInput, TChange> {

  public void Undo(TChange state) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    UndoCore(state);
  }

  public void Redo(TChange state) {
    if (Disposed)
      throw new HandlerDisposedException(GetType().Name);
    RedoCore(state);
  }

  protected abstract void UndoCore(TChange state);
  protected abstract void RedoCore(TChange state);

}