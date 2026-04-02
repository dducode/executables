using Executables.Handling;

namespace Executables;

/// <summary>
/// Represents a command that may succeed or fail.
/// </summary>
/// <typeparam name="T">Type of the command input.</typeparam>
public interface ICommand<in T> : IExecutable<T, bool> {

  /// <summary>
  /// Executes the command.
  /// </summary>
  /// <param name="input">Command input.</param>
  /// <returns><see langword="true"/> when a handler was invoked; otherwise, <see langword="false"/>.</returns>
  bool Execute(T input);

}

/// <summary>
/// Default command implementation backed by a registered handler.
/// </summary>
/// <typeparam name="T">Type of the command input.</typeparam>
public sealed class Command<T> : Handleable<T, Unit>, ICommand<T> {

  public bool Execute(T input) {
    Handler<T, Unit> handler = Handler;
    if (handler == null)
      return false;

    handler.Handle(input);
    return true;
  }

  public Executor GetExecutor() {
    return new Executor(this);
  }

  IExecutor<T, bool> IExecutable<T, bool>.GetExecutor() {
    return GetExecutor();
  }

  public readonly struct Executor(ICommand<T> command) : IExecutor<T, bool>, IEquatable<Executor> {

    private readonly ICommand<T> _command = command;

    public bool Execute(T input) {
      return _command.Execute(input);
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