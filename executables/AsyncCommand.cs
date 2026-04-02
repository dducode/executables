using Executables.Handling;

namespace Executables;

/// <summary>
/// Represents an asynchronous command that may succeed or fail.
/// </summary>
/// <typeparam name="T">Type of the command input.</typeparam>
public interface IAsyncCommand<in T> : IAsyncExecutable<T, bool> {

  /// <summary>
  /// Executes the command.
  /// </summary>
  /// <param name="input">Command input.</param>
  /// <param name="token">Cancellation token.</param>
  /// <returns><see langword="true"/> when a handler completed successfully; otherwise, <see langword="false"/>.</returns>
  ValueTask<bool> Execute(T input, CancellationToken token = default);

}

/// <summary>
/// Default asynchronous command implementation backed by a registered handler.
/// </summary>
/// <typeparam name="T">Type of the command input.</typeparam>
public sealed class AsyncCommand<T> : AsyncHandleable<T, Unit>, IAsyncCommand<T> {

  public ValueTask<bool> Execute(T input, CancellationToken token = default) {
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

  public readonly struct Executor(IAsyncCommand<T> command) : IAsyncExecutor<T, bool>, IEquatable<Executor> {

    private readonly IAsyncCommand<T> _command = command;

    public ValueTask<bool> Execute(T input, CancellationToken token = default) {
      return _command.Execute(input, token);
    }

    public bool Equals(Executor other) {
      return _command.Equals(other._command);
    }

    public override bool Equals(object obj) {
      return obj is Executor other && Equals(other);
    }

    public override int GetHashCode() {
      return _command.GetHashCode();
    }

    public static bool operator ==(Executor left, Executor right) {
      return left.Equals(right);
    }

    public static bool operator !=(Executor left, Executor right) {
      return !(left == right);
    }

  }

}