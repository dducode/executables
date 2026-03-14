namespace Interactions.Core.Executables;

internal sealed class ExecutableFunc<T1, T2>(Func<T1, T2> func) : IExecutable<T1, T2>, IExecutor<T1, T2> {

  public IExecutor<T1, T2> GetExecutor() {
    return this;
  }

  T2 IExecutor<T1, T2>.Execute(T1 input) {
    return func(input);
  }

}

internal sealed class ExecutableFunc<T>(Func<T> func) : IExecutable<Unit, T>, IExecutor<Unit, T> {

  public IExecutor<Unit, T> GetExecutor() {
    return this;
  }

  T IExecutor<Unit, T>.Execute(Unit input) {
    return func();
  }

}