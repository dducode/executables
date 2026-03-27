using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Internal;

namespace Interactions.Handling;

public static class ReversibleHandler {

  [Pure]
  public static ReversibleHandler<T1, T2> Create<T1, T2>(Func<T1, T2> execution, Action<T2> undo, Action<T2> redo) {
    ExceptionsHelper.ThrowIfNull(execution, nameof(execution));
    ExceptionsHelper.ThrowIfNull(undo, nameof(undo));
    ExceptionsHelper.ThrowIfNull(redo, nameof(redo));
    return new AnonymousReversibleHandler<T1, T2>(execution, undo, redo);
  }

  [Pure]
  public static ReversibleHandler<T, T> Create<T>(Action<T> execution, Action<T> undo, Action<T> redo) {
    ExceptionsHelper.ThrowIfNull(execution, nameof(execution));
    ExceptionsHelper.ThrowIfNull(undo, nameof(undo));
    ExceptionsHelper.ThrowIfNull(redo, nameof(redo));
    return new AnonymousReversibleHandler<T, T>(i => {
      execution(i);
      return i;
    }, undo, redo);
  }

  [Pure]
  public static ReversibleHandler<T, T> Create<T>(Action<T> execution, Action<T> undo) {
    ExceptionsHelper.ThrowIfNull(execution, nameof(execution));
    ExceptionsHelper.ThrowIfNull(undo, nameof(undo));
    return new AnonymousReversibleHandler<T, T>(i => {
      execution(i);
      return i;
    }, undo, execution);
  }

}