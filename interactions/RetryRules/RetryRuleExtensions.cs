using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

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
  /// Appends attempt-count based retry behavior to current rule.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="time">Delay for retry.</param>
  /// <param name="maxAttemptsCount">Maximum allowed failed attempts.</param>
  /// <returns>Composite retry rule.</returns>
  [Pure]
  public static IRetryRule<TEx> Simple<TEx>(this IRetryRule<TEx> rule, TimeSpan time, int maxAttemptsCount) where TEx : Exception {
    return rule.Compose(RetryRule.Simple<TEx>(time, maxAttemptsCount));
  }

  /// <summary>
  /// Appends exponential-backoff retry behavior to current rule.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="startTime">Initial delay for first retry.</param>
  /// <param name="maxAttempts">Maximum allowed failed attempts.</param>
  /// <returns>Composite retry rule.</returns>
  [Pure]
  public static IRetryRule<TEx> Exponential<TEx>(this IRetryRule<TEx> rule, TimeSpan startTime, int maxAttempts)
    where TEx : Exception {
    return rule.Compose(RetryRule.Exponential<TEx>(startTime, maxAttempts));
  }

}
