namespace Interactions.Core.Executables;

internal sealed class AsyncVoidExecutable<T>(IAsyncExecutable<T, Unit> inner) : IAsyncExecutable<T> {

  public async ValueTask Execute(T input, CancellationToken token = default) {
    await inner.Execute(input, token);
  }

}