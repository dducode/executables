using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Commands;

public static class CommandsExtensions {

  public static bool Execute(this ICommand<Unit> command) {
    return command.Execute(default);
  }

  [Pure]
  public static IAsyncCommand<T> ToAsyncCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return new AsyncProxyCommand<T>(command);
  }

}