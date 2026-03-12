namespace Interactions.Core.Executables;

internal sealed class ExecutableAsyncAction<T>(AsyncAction<T> action) : IAsyncExecutable<T> {

  public ValueTask Execute(T input, CancellationToken token = default) {
    return action(input, token);
  }

}

internal sealed class ExecutableAsyncAction(AsyncAction action) : IAsyncExecutable<Unit> {

  public ValueTask Execute(Unit input, CancellationToken token = default) {
    return action(token);
  }

}