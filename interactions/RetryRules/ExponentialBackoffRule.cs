namespace Interactions.RetryRules;

internal sealed class ExponentialBackoffRule<TEx>(TimeSpan startTime, int maxAttempts) : IRetryRule<TEx> where TEx : Exception {

  public async ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    if (attemptsCount > maxAttempts)
      return false;

    await Task.Delay((int)(startTime.TotalMilliseconds * Math.Pow(2, attemptsCount - 1)), token);
    return true;
  }

}