namespace Interactions.Core;

public interface IAsyncQuery<in T1, T2> : IAsyncExecutable<T1, T2>;

public class AsyncQuery<T1, T2> : AsyncHandleable<T1, T2>, IAsyncQuery<T1, T2> {

  public virtual ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    AsyncHandler<T1, T2> handler = Handler;
    if (handler == null)
      throw new MissingHandlerException("Cannot handle query");
    token.ThrowIfCancellationRequested();
    return handler.Execute(input, token);
  }

}