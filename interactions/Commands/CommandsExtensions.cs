using System.Diagnostics.Contracts;
using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Commands;

public static partial class CommandsExtensions {

  [Pure]
  public static Command<T> Compose<T>(this Command<T> command, Command<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new CompositeCommand<T>(command, other);
  }

  [Pure]
  public static AsyncCommand<T> Compose<T>(this AsyncCommand<T> command, AsyncCommand<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncCompositeCommand<T>(command, other);
  }

  public static bool Execute<T>(this ICommand<T> command, T input, Action<InteractionContext> init) {
    ExceptionsHelper.ThrowIfNull(init, nameof(init));

    IReadonlyContext previous = InteractionContext.Current;
    using var current = new InteractionContext(previous);
    init(current);
    InteractionContext.Current = current;

    try {
      return command.Execute(input);
    }
    finally {
      InteractionContext.Current = previous;
    }
  }

  public static bool Execute(this ICommand<Unit> command, Action<InteractionContext> init) {
    return command.Execute(default, init);
  }

}