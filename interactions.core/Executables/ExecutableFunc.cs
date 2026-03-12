namespace Interactions.Core.Executables;

internal sealed class ExecutableFunc<T1, T2>(Func<T1, T2> func) : IExecutable<T1, T2> {

  public T2 Execute(T1 input) {
    return func(input);
  }

}

internal sealed class ExecutableFunc<T>(Func<T> func) : IExecutable<Unit, T> {

  public T Execute(Unit input) {
    return func();
  }

}