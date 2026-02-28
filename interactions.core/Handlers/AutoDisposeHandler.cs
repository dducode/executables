namespace Interactions.Core.Handlers;

internal sealed class AutoDisposeHandler<T1, T2, TException>(Handler<T1, T2> inner, IDisposable handle) : Handler<T1, T2> where TException : Exception {

  public override T2 Handle(T1 input) {
    ThrowIfDisposed(nameof(AutoDisposeHandler<T1, T2, TException>));

    try {
      return inner.Handle(input);
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