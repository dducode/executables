using Interactions.Core;

namespace Interactions.Handlers;

internal sealed class AsyncCatchHandler<TException, T1, T2>(
  AsyncHandler<T1, T2> inner,
  AsyncFunc<TException, T1, T2> @catch) : AsyncHandler<T1, T2> where TException : Exception {

  public override async ValueTask<T2> Handle(T1 input, CancellationToken token = default) {
    ThrowIfDisposed(nameof(AsyncCatchHandler<TException, T1, T2>));

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