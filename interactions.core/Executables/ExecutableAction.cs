namespace Interactions.Core.Executables;

internal sealed class ExecutableAction<T>(Action<T> action) : IExecutable<T, Unit>, IExecutor<T, Unit> {

  public IExecutor<T, Unit> GetExecutor() {
    return this;
  }

  Unit IExecutor<T, Unit>.Execute(T input) {
    action(input);
    return default;
  }

}

internal sealed class ExecutableAction(Action action) : IExecutable<Unit, Unit>, IExecutor<Unit, Unit> {

  public IExecutor<Unit, Unit> GetExecutor() {
    return this;
  }

  Unit IExecutor<Unit, Unit>.Execute(Unit input) {
    action();
    return default;
  }

}