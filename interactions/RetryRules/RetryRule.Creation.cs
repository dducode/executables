using System.Diagnostics.Contracts;
using Interactions.Core.RetryRules;
using Interactions.Internal;

namespace Interactions.RetryRules;

/// <summary>
/// Factory methods for building retry rules.
/// </summary>
public static class RetryRule {

  /// <summary>
  /// Creates a rule that never retries.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by this rule.</typeparam>
  /// <returns>Identity retry rule.</returns>
  [Pure]
  public static IRetryRule<TEx> Identity<TEx>() where TEx : Exception {
    return IdentityRule<TEx>.Instance;
  }

  /// <summary>
  /// Creates a rule with exponential delay between retries.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by this rule.</typeparam>
  /// <param name="startTime">Initial delay for first retry.</param>
  /// <param name="maxAttempts">Maximum allowed failed attempts.</param>
  /// <returns>Exponential backoff retry rule.</returns>
  [Pure]
  public static IRetryRule<TEx> ExponentialBackoff<TEx>(TimeSpan startTime, int maxAttempts, float factor = 2, float jitter = 0) where TEx : Exception {
    ExceptionsHelper.ThrowIfLessOrEqual(startTime, TimeSpan.Zero, nameof(startTime));
    ExceptionsHelper.ThrowIfLessOrEqual(maxAttempts, 0, nameof(maxAttempts));
    ExceptionsHelper.ThrowIfLessOrEqual(factor, 0, nameof(factor));
    ExceptionsHelper.ThrowIfOutOfRange(jitter, 0, 1, nameof(factor));
    return new ExponentialBackoffRule<TEx>(startTime, maxAttempts, factor, jitter);
  }

  /// <summary>
  /// Creates a retry rule from asynchronous delegate.
  /// </summary>
  /// <typeparam name="TEx">Exception type handled by this rule.</typeparam>
  /// <param name="rule">
  /// Delegate that receives failed-attempt count and thrown exception,
  /// and returns <see langword="true"/> to continue retrying.
  /// </param>
  /// <returns>Retry rule backed by provided delegate.</returns>
  [Pure]
  public static IRetryRule<TEx> Create<TEx>(AsyncFunc<int, TEx, bool> rule) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(rule, nameof(rule));
    return new AnonymousRule<TEx>(rule);
  }

}