using Interactions.Handling;

namespace Interactions.Core.Handlers;

internal sealed class AutoDetachableHandler<T1, T2, TException>(Handler<T1, T2> inner) : Handler<T1, T2> where TException : Exception {

  protected override T2 HandleCore(T1 input) {
    try {
      return inner.Handle(input);
    }
    catch (TException) {
      DisposeHandles();
      throw;
    }
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}