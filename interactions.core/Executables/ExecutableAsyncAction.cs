namespace Interactions.Core.Executables;

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