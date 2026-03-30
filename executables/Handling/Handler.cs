namespace Executables.Handling;

/// <summary>
/// Base class for handlers.
/// </summary>
/// <typeparam name="T1">Type of the handler input.</typeparam>
/// <typeparam name="T2">Type of the handler result.</typeparam>
public abstract class Handler<T1, T2> : DisposableHandler, IExecutable<T1, T2> {

  /// <summary>
  /// Handles an input value.
  /// </summary>
  /// <param name="input">Input value.</param>
  /// <returns>Handler result.</returns>
  /// <exception cref="HandlerDisposedException">The handler has already been disposed.</exception>
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

  /// <summary>
  /// Handles an input value without pre-checks performed by <see cref="Handle"/>.
  /// </summary>
  protected abstract T2 HandleCore(T1 input);

  public readonly struct Executor(Handler<T1, T2> handler) : IExecutor<T1, T2> {

    public T2 Execute(T1 input) {
      return handler.Handle(input);
    }

  }

}