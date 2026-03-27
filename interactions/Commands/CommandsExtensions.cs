using System.Diagnostics.Contracts;
using Interactions.Core.Commands;
using Interactions.Internal;

namespace Interactions.Commands;

public static partial class CommandsExtensions {

  public static bool Execute(this ICommand<Unit> command) {
    return command.Execute(default);
  }

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

  [Pure]
  public static IAsyncCommand<T> ToAsyncCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return new AsyncProxyCommand<T>(command);
  }

}