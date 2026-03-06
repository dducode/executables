namespace Interactions.Core.Handlers;

internal sealed class AsyncAutoDisposeHandler<T1, T2, TException>(AsyncHandler<T1, T2> inner, IDisposable handle)
  : AsyncHandler<T1, T2> where TException : Exception {

  protected override async ValueTask<T2> ExecuteCore(T1 input, CancellationToken token = default) {
    try {
      return await inner.Execute(input, token);
    }
    catch (TException) {
      handle.Dispose();
      throw;
    }
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}