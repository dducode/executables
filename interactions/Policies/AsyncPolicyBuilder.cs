using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Fallbacks;
using Interactions.Guards;
using Interactions.Operations;
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
  /// Creates a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Guard policy.</returns>
  public AsyncPolicyBuilder<T1, T2> Guard(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return Add(new AsyncGuardPolicy<T1, T2>(guard));
  }

  public AsyncPolicyBuilder<T1, T2> PreventReentrance() {
    return Add(new AsyncPreventReentrancePolicy<T1, T2>());
  }

  public AsyncPolicyBuilder<T1, T2> Fallback<TEx>(IFallbackHandler<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return Add(new AsyncFallbackPolicy<T1, T2, TEx>(fallback));
  }

  public AsyncPolicyBuilder<T1, T2> CancelAfterCompletion() {
    return Add(new CancelAfterCompletionPolicy<T1, T2>());
  }

  public AsyncPolicyBuilder<T1, T2> Create(AsyncFunc<T1, IAsyncExecutor<T1, T2>, T2> policy) {
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
      .Aggregate(executable, (current, policy) => new AsyncExecutableOperator<T1, T1, T2, T2>(policy, current));
  }

}