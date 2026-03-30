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

  public readonly struct Executor(ICommand<T> command) : IExecutor<T, bool> {

    public bool Execute(T input) {
      return command.Execute(input);
    }

  }

}