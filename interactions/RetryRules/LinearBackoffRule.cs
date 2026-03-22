namespace Interactions.RetryRules;

internal sealed class LinearBackoffRule<TEx>(TimeSpan time, int maxAttempts) : IRetryRule<TEx> where TEx : Exception {

  public async ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    if (attemptsCount > maxAttempts)
      return false;

    await Task.Delay((int)time.TotalMilliseconds, token);
    return true;
  }

}