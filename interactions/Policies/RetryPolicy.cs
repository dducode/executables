using Interactions.Core;
using Interactions.RetryRules;

namespace Interactions.Policies;

internal sealed class RetryPolicy<T1, T2, TEx>(IRetryRule<TEx> rule) : AsyncPolicy<T1, T2> where TEx : Exception {

  public override async ValueTask<T2> Execute(T1 input, IAsyncExecutable<T1, T2> executable, CancellationToken token) {
    var attempt = 0;

    do {
      try {
        return await executable.Execute(input, token);
      }
      catch (TEx e) {
        if (!await rule.ShouldRetry(++attempt, e, token))
          throw;
      }
    } while (true);
  }

}