namespace Interactions.Core.Executables;

internal sealed class ExecutableAsyncFunc<T1, T2>(AsyncFunc<T1, T2> func) : IAsyncExecutable<T1, T2>, IAsyncExecutor<T1, T2> {

  IAsyncExecutor<T1, T2> IAsyncExecutable<T1, T2>.GetExecutor() {
    return this;
  }

  ValueTask<T2> IAsyncExecutor<T1, T2>.Execute(T1 input, CancellationToken token) {
    return func(input, token);
  }

}

internal sealed class ExecutableAsyncFunc<T>(AsyncFunc<T> func) : IAsyncExecutable<Unit, T>, IAsyncExecutor<Unit, T> {

  IAsyncExecutor<Unit, T> IAsyncExecutable<Unit, T>.GetExecutor() {
    return this;
  }

  ValueTask<T> IAsyncExecutor<Unit, T>.Execute(Unit input, CancellationToken token) {
    return func(token);
  }

}