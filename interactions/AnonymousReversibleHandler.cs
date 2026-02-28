namespace Interactions;

internal sealed class AnonymousReversibleHandler<T1, T2>(Func<T1, T2> execution, Action<T2> undo, Action<T2> redo) : ReversibleHandler<T1, T2> {

  public override T2 Handle(T1 input) {
    ThrowIfDisposed(nameof(AnonymousReversibleHandler<T1, T2>));
    return execution(input);
  }

  public override void Undo(T2 state) {
    ThrowIfDisposed(nameof(AnonymousReversibleHandler<T1, T2>));
    undo(state);
  }

  public override void Redo(T2 state) {
    ThrowIfDisposed(nameof(AnonymousReversibleHandler<T1, T2>));
    redo(state);
  }

}