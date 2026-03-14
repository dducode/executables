namespace Interactions.Core.Commands;

public static partial class CommandsExtensions {

  public static ValueTask<bool> Execute(this IAsyncCommand<Unit> command, CancellationToken token = default) {
    return command.Execute(default, token);
  }

}