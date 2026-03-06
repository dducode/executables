namespace Interactions.RetryRules;

internal sealed class IdentityRule<TEx> : IRetryRule<TEx> where TEx : Exception {

  internal static IdentityRule<TEx> Instance { get; } = new();

  private IdentityRule() { }

  public ValueTask<bool> ShouldRetry(int attemptsCount, TEx exception, CancellationToken token) {
    return new ValueTask<bool>(false);
  }

}