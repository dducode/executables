namespace Interactions.Core.Executables;

internal sealed class ExecutableAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) : IExecutable<(T1, T2, T3, T4), Unit>, IExecutor<(T1, T2, T3, T4), Unit> {

  IExecutor<(T1, T2, T3, T4), Unit> IExecutable<(T1, T2, T3, T4), Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<(T1, T2, T3, T4), Unit>.Execute((T1, T2, T3, T4) input) {
    action(input.Item1, input.Item2, input.Item3, input.Item4);
    return default;
  }

}

internal sealed class ExecutableAction<T1, T2, T3>(Action<T1, T2, T3> action) : IExecutable<(T1, T2, T3), Unit>, IExecutor<(T1, T2, T3), Unit> {

  IExecutor<(T1, T2, T3), Unit> IExecutable<(T1, T2, T3), Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<(T1, T2, T3), Unit>.Execute((T1, T2, T3) input) {
    action(input.Item1, input.Item2, input.Item3);
    return default;
  }

}

internal sealed class ExecutableAction<T1, T2>(Action<T1, T2> action) : IExecutable<(T1, T2), Unit>, IExecutor<(T1, T2), Unit> {

  IExecutor<(T1, T2), Unit> IExecutable<(T1, T2), Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<(T1, T2), Unit>.Execute((T1, T2) input) {
    action(input.Item1, input.Item2);
    return default;
  }

}

internal sealed class ExecutableAction<T>(Action<T> action) : IExecutable<T, Unit>, IExecutor<T, Unit> {

  IExecutor<T, Unit> IExecutable<T, Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<T, Unit>.Execute(T input) {
    action(input);
    return default;
  }

}

internal sealed class ExecutableAction(Action action) : IExecutable<Unit, Unit>, IExecutor<Unit, Unit> {

  IExecutor<Unit, Unit> IExecutable<Unit, Unit>.GetExecutor() {
    return this;
  }

  Unit IExecutor<Unit, Unit>.Execute(Unit input) {
    action();
    return default;
  }

}