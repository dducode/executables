namespace Executables.Handling;

/// <summary>
/// Base class for asynchronous handlers.
/// </summary>
/// <typeparam name="T1">Type of the handler input.</typeparam>
/// <typeparam name="T2">Type of the handler result.</typeparam>
public abstract class AsyncHandler<T1, T2> : DisposableHandler, IAsyncExecutable<T1, T2> {

  /// <summary>
  /// Handles an input value.
  /// </summary>
  /// <param name="input">Input value.</param>
  /// <param name="token">Cancellation token.</param>
  /// <returns>Asynchronous handler result.</returns>
  /// <exception cref="HandlerDisposedException">The handler has already been disposed.</exception>
  /// <exception cref="OperationCanceledException"><paramref name="token"/> was canceled.</exception>
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

  /// <summary>
  /// Handles an input value without pre-checks performed by <see cref="Handle"/>.
  /// </summary>
  protected abstract ValueTask<T2> HandleCore(T1 input, CancellationToken token = default);

  public readonly struct Executor(AsyncHandler<T1, T2> handler) : IAsyncExecutor<T1, T2>, IEquatable<Executor> {

    private readonly AsyncHandler<T1, T2> _handler = handler;

    public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
      return _handler.Handle(input, token);
    }

    public bool Equals(Executor other) {
      return _handler.Equals(other._handler);
    }

    public override bool Equals(object obj) {
      return obj is Executor other && Equals(other);
    }

    public override int GetHashCode() {
      return _handler.GetHashCode();
    }

    public static bool operator ==(Executor left, Executor right) {
      return left.Equals(right);
    }

    public static bool operator !=(Executor left, Executor right) {
      return !(left == right);
    }

  }

}