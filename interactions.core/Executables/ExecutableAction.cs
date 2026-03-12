namespace Interactions.Core.Executables;

internal sealed class ExecutableAction<T>(Action<T> action) : IExecutable<T> {

  public void Execute(T input) {
    action(input);
  }

}

internal sealed class ExecutableAction(Action action) : IExecutable<Unit> {

  public void Execute(Unit input) {
    action();
  }

}