namespace Executables.Handling;

public abstract class Handler<T1, T2> : DisposableHandler, IExecutable<T1, T2> {

  public T2 Handle(T1 input) {
    ThrowIfDisposed();
    return HandleCore(input);
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return GetExecutor();
  }

  protected abstract T2 HandleCore(T1 input);

  public readonly struct Executor(Handler<T1, T2> handler) : IExecutor<T1, T2> {

    public T2 Execute(T1 input) {
      return handler.Handle(input);
    }

  }

}