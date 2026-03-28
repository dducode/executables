namespace Interactions.Handling;

public abstract class AsyncHandler<T1, T2> : DisposableHandler, IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed();
    token.ThrowIfCancellationRequested();
    return HandleCore(input, token);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  protected abstract ValueTask<T2> HandleCore(T1 input, CancellationToken token = default);

  public readonly struct Executor(AsyncHandler<T1, T2> handler) : IAsyncExecutor<T1, T2> {

    public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
      return handler.Handle(input, token);
    }

  }

}