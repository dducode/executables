using Executables.Policies;
using Executables.RetryRules;

namespace Executables.Core.Policies;

internal sealed class RetryPolicy<T1, T2, TEx>(IRetryRule<TEx> rule) : AsyncPolicy<T1, T2> where TEx : Exception {

  public override async ValueTask<T2> Invoke(T1 input, IAsyncExecutor<T1, T2> executor, CancellationToken token = default) {
    var attempt = 0;

    do {
      try {
        return await executor.Execute(input, token);
      }
      catch (TEx e) {
        if (!await rule.ShouldRetry(++attempt, e, token))
          throw;
      }
    } while (true);
  }

}