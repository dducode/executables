using System.Diagnostics.Contracts;
using Interactions.Core.Commands;

namespace Interactions.Core.Extensions;

public static class CommandsExtensions {

  public static bool Execute(this ICommand<Unit> command) {
    return command.Execute(default);
  }

  public static ValueTask<bool> Execute(this IAsyncCommand<Unit> command, CancellationToken token = default) {
    return command.Execute(default, token);
  }

  [Pure]
  public static IAsyncCommand<T> ToAsyncCommand<T>(this ICommand<T> command) {
    ExceptionsHelper.ThrowIfNullReference(command);
    return new AsyncProxyCommand<T>(command);
  }

}