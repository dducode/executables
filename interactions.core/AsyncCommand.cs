namespace Interactions.Core;

public interface IAsyncCommand<in T> : IAsyncExecutable<T, bool> {

  ValueTask<bool> Execute(T input, CancellationToken token = default);

}

public class AsyncCommand<T> : AsyncHandleable<T, Unit>, IAsyncCommand<T> {

  public virtual async ValueTask<bool> Execute(T input, CancellationToken token = default) {
    if (token.IsCancellationRequested)
      return false;

    AsyncHandler<T, Unit> handler = Handler;
    if (handler == null)
      return false;

    await handler.Handle(input, token);
    return true;
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IAsyncExecutor<T, bool> IAsyncExecutable<T, bool>.GetExecutor() {
    return GetExecutor();
  }

  public readonly struct Executor(IAsyncCommand<T> command) : IAsyncExecutor<T, bool> {

    public ValueTask<bool> Execute(T input, CancellationToken token = default) {
      return command.Execute(input, token);
    }

  }

}