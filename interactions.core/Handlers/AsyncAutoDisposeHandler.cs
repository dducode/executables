namespace Interactions.Core.Handlers;

internal sealed class AsyncAutoDisposeHandler<T1, T2, TException>(AsyncHandler<T1, T2> inner, IDisposable handle)
  : AsyncHandler<T1, T2> where TException : Exception {

  public override async ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncAutoDisposeHandler<T1, T2, TException>));

    try {
      return await inner.Handle(input, token);
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