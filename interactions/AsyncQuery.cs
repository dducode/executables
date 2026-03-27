using Interactions.Handling;

namespace Interactions;

public interface IAsyncQuery<in T1, T2> : IAsyncExecutable<T1, T2> {

  ValueTask<T2> Send(T1 input, CancellationToken token = default);

}

public class AsyncQuery<T1, T2> : AsyncHandleable<T1, T2>, IAsyncQuery<T1, T2> {

  public virtual ValueTask<T2> Send(T1 input, CancellationToken token = default) {
    AsyncHandler<T1, T2> handler = Handler;
    if (handler == null)
      throw new MissingHandlerException("Cannot handle query");
    token.ThrowIfCancellationRequested();
    return handler.Handle(input, token);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  public readonly struct Executor(IAsyncQuery<T1, T2> query) : IAsyncExecutor<T1, T2> {

    public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
      return query.Send(input, token);
    }

  }

}