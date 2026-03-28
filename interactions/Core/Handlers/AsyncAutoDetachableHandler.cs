using Interactions.Handling;

namespace Interactions.Core.Handlers;

internal sealed class AsyncAutoDetachableHandler<T1, T2, TException>(AsyncHandler<T1, T2> inner)
  : AsyncHandler<T1, T2> where TException : Exception {

  protected override async ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    try {
      return await inner.Handle(input, token);
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