namespace Interactions.Core.Executables;

internal sealed class AsyncExecutableHandlerWrapper<T1, T2>(IAsyncExecutable<T1, T2> inner) : AsyncHandler<T1, T2> {

  protected override ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    return inner.Execute(input, token);
  }

  protected override void DisposeCore() {
    (inner as IDisposable)?.Dispose();
  }

#if !NETFRAMEWORK
  protected override ValueTask AsyncDisposeCore() {
    return (inner as IAsyncDisposable)?.DisposeAsync() ?? default;
  }
#endif

}

internal sealed class AsyncExecutableHandlerWrapper<T>(IAsyncExecutable<T> inner) : AsyncHandler<T, Unit> {

  protected override async ValueTask<Unit> ExecuteCore(T input, CancellationToken token = default) {
    await inner.Execute(input, token);
    return default;
  }

}