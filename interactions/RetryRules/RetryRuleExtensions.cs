using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.RetryRules;

/// <summary>
/// Extension methods for composing retry rules.
/// </summary>
public static class RetryRuleExtensions {

  /// <summary>
  /// Combines two retry rules into one sequential rule chain.
  /// </summary>
  /// <typeparam name="TException">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="other">Rule to append to base rule.</param>
  /// <returns>Composite retry rule.</returns>
  [Pure]
  public static IRetryRule<TException> Compose<TException>(this IRetryRule<TException> rule, IRetryRule<TException> other) where TException : Exception {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new CompositeRule<TException>(rule, other);
  }

  /// <summary>
  /// Appends attempt-count based retry behavior to current rule.
  /// </summary>
  /// <typeparam name="TException">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="maxAttemptsCount">Maximum allowed failed attempts.</param>
  /// <returns>Composite retry rule.</returns>
  [Pure]
  public static IRetryRule<TException> Simple<TException>(this IRetryRule<TException> rule, int maxAttemptsCount) where TException : Exception {
    return rule.Compose(RetryRule.Simple<TException>(maxAttemptsCount));
  }

  /// <summary>
  /// Appends exponential-backoff retry behavior to current rule.
  /// </summary>
  /// <typeparam name="TException">Exception type handled by composed rules.</typeparam>
  /// <param name="rule">Base retry rule.</param>
  /// <param name="startTime">Initial delay for first retry.</param>
  /// <param name="maxAttempts">Maximum allowed failed attempts.</param>
  /// <returns>Composite retry rule.</returns>
  [Pure]
  public static IRetryRule<TException> Exponential<TException>(this IRetryRule<TException> rule, TimeSpan startTime, int maxAttempts)
    where TException : Exception {
    return rule.Compose(RetryRule.Exponential<TException>(startTime, maxAttempts));
  }

}
