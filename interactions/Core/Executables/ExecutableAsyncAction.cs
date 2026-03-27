namespace Interactions.Core.Executables;

internal sealed class ExecutableAsyncAction<T1, T2, T3, T4>(AsyncAction<T1, T2, T3, T4> action)
  : IAsyncExecutable<(T1, T2, T3, T4), Unit>, IAsyncExecutor<(T1, T2, T3, T4), Unit> {

  IAsyncExecutor<(T1, T2, T3, T4), Unit> IAsyncExecutable<(T1, T2, T3, T4), Unit>.GetExecutor() {
    return this;
  }

  async ValueTask<Unit> IAsyncExecutor<(T1, T2, T3, T4), Unit>.Execute((T1, T2, T3, T4) input, CancellationToken token) {
    await action(input.Item1, input.Item2, input.Item3, input.Item4, token);
    return default;
  }

}

internal sealed class ExecutableAsyncAction<T1, T2, T3>(AsyncAction<T1, T2, T3> action)
  : IAsyncExecutable<(T1, T2, T3), Unit>, IAsyncExecutor<(T1, T2, T3), Unit> {

  IAsyncExecutor<(T1, T2, T3), Unit> IAsyncExecutable<(T1, T2, T3), Unit>.GetExecutor() {
    return this;
  }

  async ValueTask<Unit> IAsyncExecutor<(T1, T2, T3), Unit>.Execute((T1, T2, T3) input, CancellationToken token) {
    await action(input.Item1, input.Item2, input.Item3, token);
    return default;
  }

}

internal sealed class ExecutableAsyncAction<T1, T2>(AsyncAction<T1, T2> action) : IAsyncExecutable<(T1, T2), Unit>, IAsyncExecutor<(T1, T2), Unit> {

  IAsyncExecutor<(T1, T2), Unit> IAsyncExecutable<(T1, T2), Unit>.GetExecutor() {
    return this;
  }

  async ValueTask<Unit> IAsyncExecutor<(T1, T2), Unit>.Execute((T1, T2) input, CancellationToken token) {
    await action(input.Item1, input.Item2, token);
    return default;
  }

}

internal sealed class ExecutableAsyncAction<T>(AsyncAction<T> action) : IAsyncExecutable<T, Unit>, IAsyncExecutor<T, Unit> {

  IAsyncExecutor<T, Unit> IAsyncExecutable<T, Unit>.GetExecutor() {
    return this;
  }

  async ValueTask<Unit> IAsyncExecutor<T, Unit>.Execute(T input, CancellationToken token) {
    await action(input, token);
    return default;
  }

}

internal sealed class ExecutableAsyncAction(AsyncAction action) : IAsyncExecutable<Unit, Unit>, IAsyncExecutor<Unit, Unit> {

  IAsyncExecutor<Unit, Unit> IAsyncExecutable<Unit, Unit>.GetExecutor() {
    return this;
  }

  async ValueTask<Unit> IAsyncExecutor<Unit, Unit>.Execute(Unit input, CancellationToken token) {
    await action(token);
    return default;
  }

}