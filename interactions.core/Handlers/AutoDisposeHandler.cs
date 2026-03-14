namespace Interactions.Core.Handlers;

internal sealed class AutoDisposeHandler<T1, T2, TException>(Handler<T1, T2> inner, IDisposable handle) : Handler<T1, T2> where TException : Exception {

  protected override T2 HandleCore(T1 input) {
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