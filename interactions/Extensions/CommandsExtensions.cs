using System.Diagnostics.Contracts;
using Interactions.Commands;
using Interactions.Core;
using Interactions.Core.Commands;

namespace Interactions.Extensions;

public static class CommandsExtensions {

  [Pure]
  public static Command<T> Compose<T>(this Command<T> command, params Command<T>[] commands) {
    ExceptionsHelper.ThrowIfEmpty(commands, nameof(commands));
    return new CompositeCommand<T>(Enumerable.Empty<Command<T>>().Append(command).Concat(commands));
  }

  [Pure]
  public static AsyncCommand<T> Compose<T>(this AsyncCommand<T> command, params AsyncCommand<T>[] commands) {
    ExceptionsHelper.ThrowIfEmpty(commands, nameof(commands));
    return new AsyncCompositeCommand<T>(Enumerable.Empty<AsyncCommand<T>>().Append(command).Concat(commands));
  }

}