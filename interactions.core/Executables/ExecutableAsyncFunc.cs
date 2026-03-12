namespace Interactions.Core.Executables;

internal sealed class ExecutableAsyncFunc<T1, T2>(AsyncFunc<T1, T2> func) : IAsyncExecutable<T1, T2> {

  public ValueTask<T2> Execute(T1 input, CancellationToken token = default) {
    return func(input, token);
  }

}

internal sealed class ExecutableAsyncFunc<T>(AsyncFunc<T> func) : IAsyncExecutable<Unit, T> {

  public ValueTask<T> Execute(Unit input, CancellationToken token = default) {
    return func(token);
  }

}