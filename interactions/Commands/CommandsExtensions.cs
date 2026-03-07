using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Commands;

namespace Interactions.Commands;

public static class CommandsExtensions {

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

}