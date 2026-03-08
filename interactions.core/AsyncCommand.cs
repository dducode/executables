namespace Interactions.Core;

public interface IAsyncCommand<in T> : IAsyncExecutable<T, bool>;

public class AsyncCommand<T> : AsyncHandleable<T, Unit>, IAsyncCommand<T> {

  public virtual async ValueTask<bool> Execute(T input, CancellationToken token = default) {
    if (token.IsCancellationRequested)
      return false;

    AsyncHandler<T, Unit> handler = Handler;
    if (handler == null)
      return false;

    await handler.Execute(input, token);
    return true;
  }

}