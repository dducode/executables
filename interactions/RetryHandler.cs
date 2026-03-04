using Interactions.Core;

namespace Interactions;

[Obsolete]
internal sealed class RetryHandler<T1, T2, TException>(
  AsyncHandler<T1, T2> inner,
  AsyncFunc<int, TException, bool> shouldRetry) : AsyncHandler<T1, T2> where TException : Exception {

  protected override async ValueTask<T2> HandleCore(T1 input, CancellationToken token = default) {
    var attempt = 0;

    do {
      try {
        return await inner.Handle(input, token);
      }
      catch (TException e) {
        if (!await shouldRetry(++attempt, e, token))
          throw;
      }
    } while (true);
  }

}