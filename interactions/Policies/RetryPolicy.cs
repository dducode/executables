using Interactions.Core;
using Interactions.RetryRules;

namespace Interactions.Policies;

internal sealed class RetryPolicy<T1, T2, TException>(IRetryRule<TException> rule) : AsyncPolicy<T1, T2> where TException : Exception {

  public override async ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token) {
    var attempt = 0;

    do {
      try {
        return await invocation.Invoke(input, token);
      }
      catch (TException e) {
        if (!await rule.ShouldRetry(++attempt, e, token))
          throw;
      }
    } while (true);
  }

}