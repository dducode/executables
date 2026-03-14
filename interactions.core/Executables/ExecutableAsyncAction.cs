namespace Interactions.Core.Executables;

internal sealed class ExecutableAsyncAction<T>(AsyncAction<T> action) : IAsyncExecutable<T, Unit> {

  public async ValueTask<Unit> Execute(T input, CancellationToken token = default) {
    await action(input, token);
    return default;
  }

}

internal sealed class ExecutableAsyncAction(AsyncAction action) : IAsyncExecutable<Unit, Unit> {

  public async ValueTask<Unit> Execute(Unit input, CancellationToken token = default) {
    await action(token);
    return default;
  }

}