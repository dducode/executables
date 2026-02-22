using Interactions.Commands;
using Interactions.Core.Commands;

namespace Interactions.Extensions;

public static class CommandContextExtensions {

  public static ICommand<T1> CreateCommand<T1, T2>(this CommandContext context, Func<T1, T2> execution, Action<T2> undo, Action<T2> redo) {
    return context.CreateCommand(ReversibleHandler.FromMethod(execution, undo, redo));
  }

  public static ICommand<T> CreateCommand<T>(this CommandContext context, Action<T> execution, Action<T> undo, Action<T> redo) {
    return context.CreateCommand(ReversibleHandler.FromMethod(execution, undo, redo));
  }

  public static ICommand<T> CreateCommand<T>(this CommandContext context, Action<T> execution, Action<T> undo) {
    return context.CreateCommand(ReversibleHandler.FromMethod(execution, undo));
  }

}