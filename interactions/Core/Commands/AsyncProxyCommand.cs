namespace Interactions.Core.Commands;

internal sealed class AsyncProxyCommand<T>(ICommand<T> command) : IAsyncCommand<T>, IAsyncExecutor<T, bool> {

  public ValueTask<bool> Execute(T input, CancellationToken token = default) {
    return new ValueTask<bool>(!token.IsCancellationRequested && command.Execute(input));
  }

  IAsyncExecutor<T, bool> IAsyncExecutable<T, bool>.GetExecutor() {
    return this;
  }

}