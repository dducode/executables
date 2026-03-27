namespace Interactions.Core.Executables;

internal sealed class AsyncTransitiveExecutable<T>(AsyncAction<T> action) : IAsyncExecutable<T, T>, IAsyncExecutor<T, T> {

  IAsyncExecutor<T, T> IAsyncExecutable<T, T>.GetExecutor() {
    return this;
  }

  async ValueTask<T> IAsyncExecutor<T, T>.Execute(T input, CancellationToken token) {
    await action(input, token);
    return input;
  }

}