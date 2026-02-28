namespace Interactions.Core.Commands;

internal sealed class AsyncProxyCommand<T>(ICommand<T> command) : IAsyncCommand<T> {

  public ValueTask<bool> Execute(T input, CancellationToken token = default) {
    return new ValueTask<bool>(!token.IsCancellationRequested && command.Execute(input));
  }

}