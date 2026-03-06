using Interactions.Core;

namespace Interactions.RetryRules;

internal sealed class AnonymousRule<TEx>(AsyncFunc<int, TEx, bool> rule) : IRetryRule<TEx> where TEx : Exception {

  public ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    return rule(attemptsCount, exception, token);
  }

}