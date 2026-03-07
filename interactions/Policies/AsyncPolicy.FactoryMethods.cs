using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core;
using Interactions.Core.Providers;
using Interactions.Core.Resolvers;
using Interactions.Fallbacks;
using Interactions.Guards;
using Interactions.RetryRules;
using Interactions.Validation;

namespace Interactions.Policies;

public abstract partial class AsyncPolicy<T1, T2> {

  /// <summary>
  /// Creates a no-op asynchronous policy that only executes wrapped invocation.
  /// </summary>
  /// <returns>Policy instance that does not alter invocation behavior.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Identity() {
    return AsyncIdentityPolicy<T1, T2>.Instance;
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Dynamic(IProvider<AsyncPolicy<T1, T2>> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new AsyncDynamicPolicy<T1, T2>(provider);
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Dynamic(Func<AsyncPolicy<T1, T2>> provider) {
    return Dynamic(Provider.Create(provider));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Lazy(IResolver<AsyncPolicy<T1, T2>> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new AsyncLazyPolicy<T1, T2>(resolver);
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Lazy(Func<AsyncPolicy<T1, T2>> resolver) {
    return Lazy(Resolver.Create(resolver));
  }

  /// <summary>
  /// Creates an asynchronous retry policy for specific exception type.
  /// </summary>
  /// <typeparam name="TEx">Exception type that can trigger retries.</typeparam>
  /// <param name="rule">Rule that decides whether the failed invocation should be retried.</param>
  /// <returns>Retry policy that handles <typeparamref name="TEx"/> failures.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Retry<TEx>(IRetryRule<TEx> rule) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(rule, nameof(rule));
    return new RetryPolicy<T1, T2, TEx>(rule);
  }

  /// <summary>
  /// Creates an asynchronous retry policy from a delegate rule.
  /// </summary>
  /// <typeparam name="TEx">Exception type that can trigger retries.</typeparam>
  /// <param name="rule">
  /// Delegate that receives current failed-attempt count and exception instance,
  /// and returns <see langword="true"/> to continue retrying.
  /// </param>
  /// <returns>Retry policy that handles <typeparamref name="TEx"/> failures.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Retry<TEx>(AsyncFunc<int, TEx, bool> rule) where TEx : Exception {
    return Retry(RetryRule.Create(rule));
  }

  /// <summary>
  /// Creates a timeout policy that limits total asynchronous invocation duration.
  /// </summary>
  /// <param name="timeout">Maximum allowed execution time.</param>
  /// <returns>Timeout policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Timeout(TimeSpan timeout) {
    ExceptionsHelper.ThrowIfLessOrEqualZero(timeout, nameof(timeout));
    return new TimeoutPolicy<T1, T2>(timeout);
  }

  /// <summary>
  /// Creates a policy that validates input before invocation and output after invocation.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for both input and output.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Validate(Validator<T1> inputValidator, Validator<T2> outputValidator) {
    ExceptionsHelper.ThrowIfNull(inputValidator, nameof(inputValidator));
    ExceptionsHelper.ThrowIfNull(outputValidator, nameof(outputValidator));
    return new AsyncValidationPolicy<T1, T2>(inputValidator, outputValidator);
  }

  /// <summary>
  /// Creates a policy that validates only invocation input.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <returns>Validation policy for input with identity output validation.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateInput(Validator<T1> inputValidator) {
    return Validate(inputValidator, Validator.Identity<T2>());
  }

  /// <summary>
  /// Creates a policy that validates only invocation output.
  /// </summary>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for output with identity input validation.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateOutput(Validator<T2> outputValidator) {
    return Validate(Validator.Identity<T1>(), outputValidator);
  }

  /// <summary>
  /// Creates an input-validation policy from a predicate.
  /// </summary>
  /// <param name="inputValidator">Predicate that returns <see langword="true"/> for valid input.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Validation policy for invocation input.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateInput(Func<T1, bool> inputValidator, string errorMessage) {
    return ValidateInput(Validator.Create(inputValidator, errorMessage));
  }

  /// <summary>
  /// Creates an output-validation policy from a predicate.
  /// </summary>
  /// <param name="outputValidator">Predicate that returns <see langword="true"/> for valid output.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Validation policy for invocation output.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> ValidateOutput(Func<T2, bool> outputValidator, string errorMessage) {
    return ValidateOutput(Validator.Create(outputValidator, errorMessage));
  }

  /// <summary>
  /// Creates a policy that reports invocation metrics.
  /// </summary>
  /// <param name="metrics">Metrics sink used to record invocation data.</param>
  /// <param name="tag">Optional logical tag used to group emitted metrics.</param>
  /// <returns>Metrics policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Metrics(IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return new AsyncMetricsPolicy<T1, T2>(metrics, tag);
  }

  /// <summary>
  /// Creates a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Guard policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Guard(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return new AsyncGuardPolicy<T1, T2>(guard);
  }

  /// <summary>
  /// Creates a guard policy from a predicate and denial message.
  /// </summary>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  /// <returns>Guard policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Guard(Func<bool> guard, string errorMessage) {
    return Guard(Guards.Guard.Create(guard, errorMessage));
  }

  /// <summary>
  /// Creates a policy that caches invocation results by input key.
  /// </summary>
  /// <param name="storage">Storage used to read and write cached values.</param>
  /// <returns>Cache policy.</returns>
  [Pure]
  public static AsyncPolicy<T1, T2> Cache(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return new AsyncCachePolicy<T1, T2>(storage);
  }

  [Pure]
  public static AsyncPolicy<T1, T2> PreventReentrance() {
    return new AsyncPreventReentrancePolicy<T1, T2>();
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Fallback<TEx>(IFallbackHandler<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return new AsyncFallbackPolicy<T1, T2, TEx>(fallback);
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Fallback<TEx>(Func<T1, TEx, T2> fallback) where TEx : Exception {
    return Fallback(FallbackHandler.Create(fallback));
  }

  [Pure]
  public static AsyncPolicy<T1, T2> Optional(Func<bool> condition, AsyncPolicy<T1, T2> other) {
    ExceptionsHelper.ThrowIfNull(condition, nameof(condition));
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return Dynamic(() => condition() ? other : Identity());
  }

}