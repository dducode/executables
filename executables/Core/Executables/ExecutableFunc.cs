namespace Executables.Core.Executables;

internal sealed class ExecutableFunc<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> func) : IExecutable<(T1, T2, T3, T4), T5>, IExecutor<(T1, T2, T3, T4), T5> {

  IExecutor<(T1, T2, T3, T4), T5> IExecutable<(T1, T2, T3, T4), T5>.GetExecutor() {
    return this;
  }

  T5 IExecutor<(T1, T2, T3, T4), T5>.Execute((T1, T2, T3, T4) input) {
    return func(input.Item1, input.Item2, input.Item3, input.Item4);
  }

}

internal sealed class ExecutableFunc<T1, T2, T3, T4>(Func<T1, T2, T3, T4> func) : IExecutable<(T1, T2, T3), T4>, IExecutor<(T1, T2, T3), T4> {

  IExecutor<(T1, T2, T3), T4> IExecutable<(T1, T2, T3), T4>.GetExecutor() {
    return this;
  }

  T4 IExecutor<(T1, T2, T3), T4>.Execute((T1, T2, T3) input) {
    return func(input.Item1, input.Item2, input.Item3);
  }

}

internal sealed class ExecutableFunc<T1, T2, T3>(Func<T1, T2, T3> func) : IExecutable<(T1, T2), T3>, IExecutor<(T1, T2), T3> {

  IExecutor<(T1, T2), T3> IExecutable<(T1, T2), T3>.GetExecutor() {
    return this;
  }

  T3 IExecutor<(T1, T2), T3>.Execute((T1, T2) input) {
    return func(input.Item1, input.Item2);
  }

}

internal sealed class ExecutableFunc<T1, T2>(Func<T1, T2> func) : IExecutable<T1, T2>, IExecutor<T1, T2> {

  IExecutor<T1, T2> IExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    return func(input);
  }

}

internal sealed class ExecutableFunc<T>(Func<T> func) : IExecutable<Unit, T>, IExecutor<Unit, T> {

  IExecutor<Unit, T> IExecutable<Unit, T>.GetExecutor() {
    return this;
  }

  T IExecutor<Unit, T>.Execute(Unit input) {
    return func();
  }

}