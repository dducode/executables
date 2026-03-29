using System.Diagnostics.Contracts;
using Interactions.Core.RetryRules;
using Interactions.Internal;

namespace Interactions.RetryRules;

/// <summary>
/// Extension methods for composing retry rules.
/// </summary>
public static class RetryRuleExtensions {

  /// <summary>
  /// Combines two retry rules into one sequential rule chain.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="other">Rule to append to base rule.</param>
  /// <returns>Composite retry rule.</returns>
  [Pure]
  public static IRetryRule<TEx> Compose<TEx>(this IRetryRule<TEx> rule, IRetryRule<TEx> other) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new CompositeRule<TEx>(rule, other);
  }

  /// <summary>
  /// Appends exponential-backoff retry behavior to current rule.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="startTime">Initial delay for first retry.</param>
  /// <param name="maxAttempts">Maximum allowed failed attempts.</param>
  /// <param name="factor">Backoff multiplier for each subsequent retry delay.</param>
  /// <param name="jitter">Randomization factor in range [0, 1).</param>
  /// <returns>Composite retry rule.</returns>
  /// <exception cref="ArgumentException">
  /// <paramref name="startTime"/> is less than or equal to zero;
  /// <paramref name="maxAttempts"/> is less than or equal to zero;
  /// or <paramref name="factor"/> is less than or equal to zero.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException"><paramref name="jitter"/> is outside [0, 1).</exception>
  [Pure]
  public static IRetryRule<TEx> ExponentialBackoff<TEx>(this IRetryRule<TEx> rule, TimeSpan startTime, int maxAttempts, float factor = 2, float jitter = 0)
    where TEx : Exception {
    return rule.Compose(RetryRule.ExponentialBackoff<TEx>(startTime, maxAttempts, factor, jitter));
  }

}