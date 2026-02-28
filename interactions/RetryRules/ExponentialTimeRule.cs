namespace Interactions.RetryRules;

internal sealed class ExponentialTimeRule<TException>(TimeSpan startTime, int maxAttempts) : IRetryRule<TException> where TException : Exception {

  public async ValueTask<bool> ShouldRetry(int attemptsCount, TException exception, CancellationToken token) {
    if (attemptsCount > maxAttempts)
      return false;

    await Task.Delay((int)(startTime.TotalMilliseconds * Math.Pow(2, attemptsCount - 1)), token);
    return true;
  }

}