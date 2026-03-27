using Interactions.Handling;

namespace Interactions.Core.Handlers;

internal sealed class AnonymousReversibleHandler<T1, T2>(Func<T1, T2> execution, Action<T2> undo, Action<T2> redo) : ReversibleHandler<T1, T2> {

  protected override T2 HandleCore(T1 input) {
    return execution(input);
  }

  protected override void UndoCore(T2 state) {
    undo(state);
  }

  protected override void RedoCore(T2 state) {
    redo(state);
  }

}