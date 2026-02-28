namespace Interactions.RetryRules;

internal sealed class SimpleRule<TException>(int maxAttempts) : IRetryRule<TException> where TException : Exception {

  public ValueTask<bool> ShouldRetry(int attemptsCount, TException exception, CancellationToken token) {
    return new ValueTask<bool>(attemptsCount <= maxAttempts);
  }

}