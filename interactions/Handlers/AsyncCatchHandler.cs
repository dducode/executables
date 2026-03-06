using Interactions.Core;

namespace Interactions.Handlers;

[Obsolete]
internal sealed class AsyncCatchHandler<TException, T1, T2>(
  AsyncHandler<T1, T2> inner,
  AsyncFunc<TException, T1, T2> @catch) : AsyncHandler<T1, T2> where TException : Exception {

  protected override async ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    try {
      return await inner.Handle(input, token);
    }
    catch (TException e) {
      return await @catch(e, input, token);
    }
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}