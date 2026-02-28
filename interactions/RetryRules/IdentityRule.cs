namespace Interactions.RetryRules;

internal sealed class IdentityRule<TException> : IRetryRule<TException> where TException : Exception {

  internal static IdentityRule<TException> Instance { get; } = new();

  private IdentityRule() { }

  public ValueTask<bool> ShouldRetry(int attemptsCount, TException exception, CancellationToken token) {
    return new ValueTask<bool>(false);
  }

}