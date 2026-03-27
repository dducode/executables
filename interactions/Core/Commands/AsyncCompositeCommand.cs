namespace Interactions.Core.Commands;

internal sealed class AsyncCompositeCommand<T>(IAsyncCommand<T> first, IAsyncCommand<T> second) : IAsyncCommand<T>, IAsyncExecutor<T, bool> {

  public ValueTask<bool> Execute(T input, CancellationToken token = default) {
    ValueTask<bool> firstTask = first.Execute(input, token);

    if (firstTask.IsCompleted)
      return firstTask.Result ? second.Execute(input, token) : new ValueTask<bool>(false);

    return Await(input, firstTask, second, token);
  }

  IAsyncExecutor<T, bool> IAsyncExecutable<T, bool>.GetExecutor() {
    return this;
  }

  private static async ValueTask<bool> Await(T input, ValueTask<bool> task, IAsyncCommand<T> secondCommand, CancellationToken token) {
    return await task && await secondCommand.Execute(input, token);
  }

}