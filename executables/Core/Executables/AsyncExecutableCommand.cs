namespace Executables.Core.Executables;

internal sealed class AsyncExecutableCommand<T>(IAsyncExecutable<T, bool> inner) : IAsyncCommand<T>, IAsyncExecutor<T, bool> {

  private readonly IAsyncExecutor<T, bool> _inner = inner.GetExecutor();

  public ValueTask<bool> Execute(T input, CancellationToken token) {
    return _inner.Execute(input, token);
  }

  IAsyncExecutor<T, bool> IAsyncExecutable<T, bool>.GetExecutor() {
    return this;
  }

}