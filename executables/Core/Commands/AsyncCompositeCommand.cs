namespace Executables.Core.Commands;

internal sealed class AsyncCompositeCommand<T>(IAsyncCommand<T> first, IAsyncCommand<T> second) : IAsyncCommand<T>, IAsyncExecutor<T, bool> {

  public ValueTask<bool> Execute(T input, CancellationToken token = default) {
    if (token.IsCancellationRequested)
      return new ValueTask<bool>(false);

    ValueTask<bool> firstTask;

    try {
      firstTask = second.Execute(input, token);
      if (firstTask.IsCompleted)
        return firstTask.Result ? first.Execute(input, token) : new ValueTask<bool>(false);
    }
    catch (OperationCanceledException) {
      return new ValueTask<bool>(false);
    }

    return Await(input, firstTask, first, token);
  }

  IAsyncExecutor<T, bool> IAsyncExecutable<T, bool>.GetExecutor() {
    return this;
  }

  private static async ValueTask<bool> Await(T input, ValueTask<bool> first, IAsyncCommand<T> secondCommand, CancellationToken token) {
    try {
      return await first && await secondCommand.Execute(input, token);
    }
    catch (OperationCanceledException) {
      return false;
    }
  }

}