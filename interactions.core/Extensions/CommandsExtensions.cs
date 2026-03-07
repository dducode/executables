using System.Diagnostics.Contracts;
using Interactions.Core.Commands;

namespace Interactions.Core.Extensions;

public static class CommandsExtensions {

  [Pure]
  public static IAsyncCommand<T> ToAsyncCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return new AsyncProxyCommand<T>(command);
  }

}