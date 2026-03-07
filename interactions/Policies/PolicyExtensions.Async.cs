using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Core.Providers;
using Interactions.Core.Resolvers;
using Interactions.Fallbacks;
using Interactions.Guards;
using Interactions.RetryRules;
using Interactions.Validation;

namespace Interactions.Policies;

public static partial class PolicyExtensions {

  /// <summary>
  /// Composes two policies so that <paramref name="other"/> wraps <paramref name="policy"/>.
  /// The first added policy runs first.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="other">Policy added inside base policy.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Compose<T1, T2>(this AsyncPolicy<T1, T2> policy, AsyncPolicy<T1, T2> other) {
    policy.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncCompositePolicy<T1, T2>(policy, other);
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Dynamic<T1, T2>(this AsyncPolicy<T1, T2> policy, IProvider<AsyncPolicy<T1, T2>> provider) {
    return policy.Compose(AsyncPolicy<T1, T2>.Dynamic(provider));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Dynamic<T1, T2>(this AsyncPolicy<T1, T2> policy, Func<AsyncPolicy<T1, T2>> provider) {
    return policy.Dynamic(Provider.Create(provider));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Lazy<T1, T2>(this AsyncPolicy<T1, T2> policy, IResolver<AsyncPolicy<T1, T2>> resolver) {
    return policy.Compose(AsyncPolicy<T1, T2>.Lazy(resolver));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Lazy<T1, T2>(this AsyncPolicy<T1, T2> policy, Func<AsyncPolicy<T1, T2>> resolver) {
    return policy.Lazy(Resolver.Create(resolver));
  }

  /// <summary>
  /// Adds retry behavior to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <typeparam name="TEx">Exception type that can trigger retries.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="rule">Rule that decides whether the failed invocation should be retried.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Retry<T1, T2, TEx>(this AsyncPolicy<T1, T2> policy, IRetryRule<TEx> rule)
    where TEx : Exception {
    return policy.Compose(AsyncPolicy<T1, T2>.Retry(rule));
  }

  /// <summary>
  /// Adds retry behavior to current policy chain using a delegate rule.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <typeparam name="TException">Exception type that can trigger retries.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="rule">
  /// Delegate that receives current failed-attempt count and exception instance,
  /// and returns <see langword="true"/> to continue retrying.
  /// </param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Retry<T1, T2, TException>(this AsyncPolicy<T1, T2> policy, AsyncFunc<int, TException, bool> rule)
    where TException : Exception {
    return policy.Retry(RetryRule.Create(rule));
  }

  /// <summary>
  /// Adds timeout behavior to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="timeout">Maximum allowed execution time.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Timeout<T1, T2>(this AsyncPolicy<T1, T2> policy, TimeSpan timeout) {
    return policy.Compose(AsyncPolicy<T1, T2>.Timeout(timeout));
  }

  /// <summary>
  /// Adds input and output validation to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Validate<T1, T2>(this AsyncPolicy<T1, T2> policy, Validator<T1> inputValidator, Validator<T2> outputValidator) {
    return policy.Compose(AsyncPolicy<T1, T2>.Validate(inputValidator, outputValidator));
  }

  /// <summary>
  /// Adds input validation to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateInput<T1, T2>(this AsyncPolicy<T1, T2> policy, Validator<T1> inputValidator) {
    return policy.Validate(inputValidator, Validator.Identity<T2>());
  }

  /// <summary>
  /// Adds output validation to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateOutput<T1, T2>(this AsyncPolicy<T1, T2> policy, Validator<T2> outputValidator) {
    return policy.Validate(Validator.Identity<T1>(), outputValidator);
  }

  /// <summary>
  /// Adds input validation to current policy chain using a predicate.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="inputValidator">Predicate that returns <see langword="true"/> for valid input.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateInput<T1, T2>(this AsyncPolicy<T1, T2> policy, Func<T1, bool> inputValidator, string errorMessage) {
    return policy.ValidateInput(Validator.Create(inputValidator, errorMessage));
  }

  /// <summary>
  /// Adds output validation to current policy chain using a predicate.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="outputValidator">Predicate that returns <see langword="true"/> for valid output.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateOutput<T1, T2>(this AsyncPolicy<T1, T2> policy, Func<T2, bool> outputValidator, string errorMessage) {
    return policy.ValidateOutput(Validator.Create(outputValidator, errorMessage));
  }

  /// <summary>
  /// Adds metrics recording to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="metrics">Metrics sink used to record invocation data.</param>
  /// <param name="tag">Optional logical tag used to group emitted metrics.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Metrics<T1, T2>(this AsyncPolicy<T1, T2> policy, IMetrics<T1, T2> metrics, string tag = null) {
    return policy.Compose(AsyncPolicy<T1, T2>.Metrics(metrics, tag));
  }

  /// <summary>
  /// Adds guard check to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Guard<T1, T2>(this AsyncPolicy<T1, T2> policy, Guard guard) {
    return policy.Compose(AsyncPolicy<T1, T2>.Guard(guard));
  }

  /// <summary>
  /// Adds guard check to current policy chain using a predicate.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Guard<T1, T2>(this AsyncPolicy<T1, T2> policy, Func<bool> guard, string errorMessage) {
    return policy.Guard(Guards.Guard.Create(guard, errorMessage));
  }

  /// <summary>
  /// Adds caching to current policy chain.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="storage">Storage used to read and write cached values.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Cache<T1, T2>(this AsyncPolicy<T1, T2> policy, ICacheStorage<T1, T2> storage) {
    return policy.Compose(AsyncPolicy<T1, T2>.Cache(storage));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> PreventReentrance<T1, T2>(this AsyncPolicy<T1, T2> policy) {
    return policy.Compose(AsyncPolicy<T1, T2>.PreventReentrance());
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Fallback<T1, T2, TException>(this AsyncPolicy<T1, T2> policy, IFallbackHandler<T1, TException, T2> fallback)
    where TException : Exception {
    return policy.Compose(AsyncPolicy<T1, T2>.Fallback(fallback));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Fallback<T1, T2, TException>(this AsyncPolicy<T1, T2> policy, Func<T1, TException, T2> fallback)
    where TException : Exception {
    return policy.Fallback(FallbackHandler.Create(fallback));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Optional<T1, T2>(this AsyncPolicy<T1, T2> policy, Func<bool> condition, AsyncPolicy<T1, T2> other) {
    return policy.Compose(AsyncPolicy<T1, T2>.Optional(condition, other));
  }

}