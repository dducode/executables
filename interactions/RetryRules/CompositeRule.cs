namespace Interactions.RetryRules;

internal sealed class CompositeRule<TException>(IRetryRule<TException> first, IRetryRule<TException> second)
  : IRetryRule<TException> where TException : Exception {

  public async ValueTask<bool> ShouldRetry(int attemptsCount, TException exception, CancellationToken token) {
    return await first.ShouldRetry(attemptsCount, exception, token) || await second.ShouldRetry(attemptsCount, exception, token);
  }

}