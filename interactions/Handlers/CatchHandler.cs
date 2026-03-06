using Interactions.Core;

namespace Interactions.Handlers;

[Obsolete]
internal sealed class CatchHandler<TException, T1, T2>(
  Handler<T1, T2> inner,
  Func<TException, T1, T2> @catch) : Handler<T1, T2> where TException : Exception {

  protected override T2 HandleCore(T1 input) {
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