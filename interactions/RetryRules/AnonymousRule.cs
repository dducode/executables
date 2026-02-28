using Interactions.Core;

namespace Interactions.RetryRules;

internal sealed class AnonymousRule<TException>(AsyncFunc<int, TException, bool> rule) : IRetryRule<TException> where TException : Exception {

  public ValueTask<bool> ShouldRetry(int attemptsCount, TException exception, CancellationToken token) {
    return rule(attemptsCount, exception, token);
  }

}