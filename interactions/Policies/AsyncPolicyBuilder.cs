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

public class AsyncPolicyBuilder<T1, T2> {

  private readonly List<AsyncPolicy<T1, T2>> _policies = [];

  internal AsyncPolicyBuilder() { }

  /// <summary>
  /// Creates a no-op asynchronous policy that only executes wrapped invocation.
  /// </summary>
  /// <returns>Policy instance that does not alter invocation behavior.</returns>
  public AsyncPolicyBuilder<T1, T2> Identity() {
    return Add(AsyncIdentityPolicy<T1, T2>.Instance);
  }

  public AsyncPolicyBuilder<T1, T2> Dynamic(IProvider<AsyncPolicy<T1, T2>> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return Add(new AsyncDynamicPolicy<T1, T2>(provider));
  }

  public AsyncPolicyBuilder<T1, T2> Lazy(IResolver<AsyncPolicy<T1, T2>> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return Add(new AsyncLazyPolicy<T1, T2>(resolver));
  }

  /// <summary>
  /// Creates an asynchronous retry policy for specific exception type.
  /// </summary>
  /// <typeparam name="TEx">Exception type that can trigger retries.</typeparam>
  /// <param name="rule">Rule that decides whether the failed invocation should be retried.</param>
  /// <returns>Retry policy that handles <typeparamref name="TEx"/> failures.</returns>
  public AsyncPolicyBuilder<T1, T2> Retry<TEx>(IRetryRule<TEx> rule) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(rule, nameof(rule));
    return Add(new RetryPolicy<T1, T2, TEx>(rule));
  }

  /// <summary>
  /// Creates a timeout policy that limits total asynchronous invocation duration.
  /// </summary>
  /// <param name="timeout">Maximum allowed execution time.</param>
  /// <returns>Timeout policy.</returns>
  public AsyncPolicyBuilder<T1, T2> Timeout(TimeSpan timeout) {
    ExceptionsHelper.ThrowIfLessOrEqualZero(timeout, nameof(timeout));
    return Add(new TimeoutPolicy<T1, T2>(timeout));
  }

  /// <summary>
  /// Creates a policy that validates input before invocation and output after invocation.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for both input and output.</returns>
  public AsyncPolicyBuilder<T1, T2> Validate(Validator<T1> inputValidator, Validator<T2> outputValidator) {
    ExceptionsHelper.ThrowIfNull(inputValidator, nameof(inputValidator));
    ExceptionsHelper.ThrowIfNull(outputValidator, nameof(outputValidator));
    return Add(new AsyncValidationPolicy<T1, T2>(inputValidator, outputValidator));
  }

  /// <summary>
  /// Creates a policy that reports invocation metrics.
  /// </summary>
  /// <param name="metrics">Metrics sink used to record invocation data.</param>
  /// <param name="tag">Optional logical tag used to group emitted metrics.</param>
  /// <returns>Metrics policy.</returns>
  public AsyncPolicyBuilder<T1, T2> Metrics(IMetrics<T1, T2> metrics, string tag = null) {
    ExceptionsHelper.ThrowIfNull(metrics, nameof(metrics));
    return Add(new AsyncMetricsPolicy<T1, T2>(metrics, tag));
  }

  /// <summary>
  /// Creates a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Guard policy.</returns>
  public AsyncPolicyBuilder<T1, T2> Guard(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return Add(new AsyncGuardPolicy<T1, T2>(guard));
  }

  /// <summary>
  /// Creates a policy that caches invocation results by input key.
  /// </summary>
  /// <param name="storage">Storage used to read and write cached values.</param>
  /// <returns>Cache policy.</returns>
  public AsyncPolicyBuilder<T1, T2> Cache(ICacheStorage<T1, T2> storage) {
    ExceptionsHelper.ThrowIfNull(storage, nameof(storage));
    return Add(new AsyncCachePolicy<T1, T2>(storage));
  }

  public AsyncPolicyBuilder<T1, T2> PreventReentrance() {
    return Add(new AsyncPreventReentrancePolicy<T1, T2>());
  }

  public AsyncPolicyBuilder<T1, T2> Fallback<TEx>(IFallbackHandler<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return Add(new AsyncFallbackPolicy<T1, T2, TEx>(fallback));
  }

  public AsyncPolicyBuilder<T1, T2> Create(AsyncFunc<T1, IAsyncExecutable<T1, T2>, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return Add(new AsyncAnonymousPolicy<T1, T2>(policy));
  }

  public AsyncPolicyBuilder<T1, T2> Add(AsyncPolicy<T1, T2> policy) {
    _policies.Add(policy);
    return this;
  }

  [Pure]
  public IAsyncExecutable<T1, T2> Apply(IAsyncExecutable<T1, T2> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return _policies
      .AsEnumerable()
      .Reverse()
      .Aggregate(executable, (current, policy) => new AsyncExecutablePolicy<T1, T2>(current, policy));
  }

}