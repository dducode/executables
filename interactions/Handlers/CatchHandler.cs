using Interactions.Core;

namespace Interactions.Handlers;

internal sealed class CatchHandler<TException, T1, T2>(
  Handler<T1, T2> inner,
  Func<TException, T1, T2> @catch) : Handler<T1, T2> where TException : Exception {

  public override T2 Handle(T1 input) {
    ThrowIfDisposed(nameof(CatchHandler<TException, T1, T2>));

    try {
      return inner.Handle(input);
    }
    catch (TException e) {
      return @catch(e, input);
    }
  }

  protected override void DisposeCore() {
    inner.Dispose();
  }

}