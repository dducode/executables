namespace Interactions.RetryRules;

internal sealed class ExponentialBackoffRule<TEx>(TimeSpan startTime, int maxAttempts, float factor, float jitter) : IRetryRule<TEx> where TEx : Exception {

  private readonly Random _random = new();

  public async ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    if (attemptsCount > maxAttempts)
      return false;

    var delay = (int)(startTime.TotalMilliseconds * Math.Pow(factor, attemptsCount - 1));
    delay += (int)(delay * ((_random.NextDouble() * 2 - 1) * jitter));
    await Task.Delay(delay, token);
    return true;
  }

}