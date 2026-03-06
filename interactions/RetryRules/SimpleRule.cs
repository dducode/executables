namespace Interactions.RetryRules;

internal sealed class SimpleRule<TEx>(int maxAttempts) : IRetryRule<TEx> where TEx : Exception {

  public ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    return new ValueTask<bool>(attemptsCount <= maxAttempts);
  }

}