namespace Interactions.RetryRules;

internal sealed class CompositeRule<TEx>(IRetryRule<TEx> first, IRetryRule<TEx> second) : IRetryRule<TEx> where TEx : Exception {

  public async ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    return await first.ShouldRetry(attemptsCount, exception, token) || await second.ShouldRetry(attemptsCount, exception, token);
  }

}