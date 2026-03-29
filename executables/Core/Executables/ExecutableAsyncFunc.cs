namespace Executables.Core.Executables;

internal sealed class ExecutableAsyncFunc<T1, T2, T3, T4, T5>(AsyncFunc<T1, T2, T3, T4, T5> func)
  : IAsyncExecutable<(T1, T2, T3, T4), T5>, IAsyncExecutor<(T1, T2, T3, T4), T5> {

  IAsyncExecutor<(T1, T2, T3, T4), T5> IAsyncExecutable<(T1, T2, T3, T4), T5>.GetExecutor() {
    return this;
  }

  ValueTask<T5> IAsyncExecutor<(T1, T2, T3, T4), T5>.Execute((T1, T2, T3, T4) input, CancellationToken token) {
    return func(input.Item1, input.Item2, input.Item3, input.Item4, token);
  }

}

internal sealed class ExecutableAsyncFunc<T1, T2, T3, T4>(AsyncFunc<T1, T2, T3, T4> func)
  : IAsyncExecutable<(T1, T2, T3), T4>, IAsyncExecutor<(T1, T2, T3), T4> {

  IAsyncExecutor<(T1, T2, T3), T4> IAsyncExecutable<(T1, T2, T3), T4>.GetExecutor() {
    return this;
  }

  ValueTask<T4> IAsyncExecutor<(T1, T2, T3), T4>.Execute((T1, T2, T3) input, CancellationToken token) {
    return func(input.Item1, input.Item2, input.Item3, token);
  }

}

internal sealed class ExecutableAsyncFunc<T1, T2, T3>(AsyncFunc<T1, T2, T3> func) : IAsyncExecutable<(T1, T2), T3>, IAsyncExecutor<(T1, T2), T3> {

  IAsyncExecutor<(T1, T2), T3> IAsyncExecutable<(T1, T2), T3>.GetExecutor() {
    return this;
  }

  ValueTask<T3> IAsyncExecutor<(T1, T2), T3>.Execute((T1, T2) input, CancellationToken token) {
    return func(input.Item1, input.Item2, token);
  }

}

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