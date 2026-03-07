using System.Diagnostics.Contracts;

namespace Interactions.Core.Commands;

public static class CommandsExtensions {

  [Pure]
  public static IAsyncCommand<T> ToAsyncCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return new AsyncProxyCommand<T>(command);
  }

}