namespace Executables.Core.Executables;

internal sealed class AsyncIdentityExecutable<T> : IAsyncExecutable<T, T>, IAsyncExecutor<T, T> {

  internal static AsyncIdentityExecutable<T> Instance { get; } = new();

  private AsyncIdentityExecutable() { }

  IAsyncExecutor<T, T> IAsyncExecutable<T, T>.GetExecutor() {
    return this;
  }

  ValueTask<T> IAsyncExecutor<T, T>.Execute(T input, CancellationToken token) {
    return new ValueTask<T>(input);
  }

}