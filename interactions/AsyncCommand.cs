using Interactions.Handling;

namespace Interactions;

public interface IAsyncCommand<in T> : IAsyncExecutable<T, bool> {

  ValueTask<bool> Execute(T input, CancellationToken token = default);

}

public class AsyncCommand<T> : AsyncHandleable<T, Unit>, IAsyncCommand<T> {

  public virtual ValueTask<bool> Execute(T input, CancellationToken token = default) {
    if (token.IsCancellationRequested)
      return new ValueTask<bool>(false);

    AsyncHandler<T, Unit> handler = Handler;
    if (handler == null)
      return new ValueTask<bool>(false);

    return Await(input, handler, token);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IAsyncExecutor<T, bool> IAsyncExecutable<T, bool>.GetExecutor() {
    return GetExecutor();
  }

  private static async ValueTask<bool> Await(T input, AsyncHandler<T, Unit> handler, CancellationToken token) {
    try {
      await handler.Handle(input, token);
      return true;
    }
    catch (OperationCanceledException) {
      return false;
    }
  }

  public readonly struct Executor(IAsyncCommand<T> command) : IAsyncExecutor<T, bool> {

    public ValueTask<bool> Execute(T input, CancellationToken token = default) {
      return command.Execute(input, token);
    }

  }

}