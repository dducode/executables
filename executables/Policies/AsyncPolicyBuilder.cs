using System.Diagnostics.Contracts;
using Executables.Core.Policies;
using Executables.Guards;
using Executables.Internal;
using Executables.Operations;
using Executables.RetryRules;
using Executables.Validation;

namespace Executables.Policies;

/// <summary>
/// Builds a composed asynchronous policy pipeline.
/// </summary>
/// <typeparam name="T1">Input type of the target executable.</typeparam>
/// <typeparam name="T2">Output type of the target executable.</typeparam>
/// <remarks>
/// Policies are invoked in reverse order of addition: the last added policy executes first.
/// </remarks>
public readonly struct AsyncPolicyBuilder<T1, T2>() {

  private readonly List<AsyncPolicy<T1, T2>> _policies = [];

  /// <summary>
  /// Adds an asynchronous retry policy for specific exception type.
  /// </summary>
  /// <typeparam name="TEx">Exception type that can trigger retries.</typeparam>
  /// <param name="rule">Rule that decides whether the failed invocation should be retried.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Retry<TEx>(IRetryRule<TEx> rule) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(rule, nameof(rule));
    return Add(new RetryPolicy<T1, T2, TEx>(rule));
  }

  /// <summary>
  /// Creates an asynchronous retry policy from a delegate rule.
  /// </summary>
  /// <param name="rule">
  /// Delegate that receives current failed-attempt count and exception instance,
  /// and returns <see langword="true"/> to continue retrying.
  /// </param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Retry<TEx>(AsyncFunc<int, TEx, bool> rule) where TEx : Exception {
    return Retry(RetryRule.Create(rule));
  }

  /// <summary>
  /// Adds a timeout policy that limits total asynchronous invocation duration.
  /// </summary>
  /// <param name="timeout">Maximum allowed execution time.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Timeout(TimeSpan timeout) {
    ExceptionsHelper.ThrowIfLessOrEqual(timeout, TimeSpan.Zero, nameof(timeout));
    return Add(new TimeoutPolicy<T1, T2>(timeout));
  }

  /// <summary>
  /// Adds a policy that validates input before invocation and output after invocation.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Validate(Validator<T1> inputValidator, Validator<T2> outputValidator) {
    ExceptionsHelper.ThrowIfNull(inputValidator, nameof(inputValidator));
    ExceptionsHelper.ThrowIfNull(outputValidator, nameof(outputValidator));
    return Add(new AsyncValidationPolicy<T1, T2>(inputValidator, outputValidator));
  }

  /// <summary>
  /// Adds a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Guard(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return Add(new AsyncGuardPolicy<T1, T2>(guard));
  }

  /// <summary>
  /// Adds a policy that rejects reentrant asynchronous execution.
  /// </summary>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> PreventReentrance() {
    return Add(new AsyncPreventReentrancePolicy<T1, T2>());
  }

  /// <summary>
  /// Creates a fallback policy from a delegate.
  /// </summary>
  /// <param name="fallback">Delegate that converts input and exception into a fallback result.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Fallback<TEx>(Func<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return Add(new AsyncFallbackPolicy<T1, T2, TEx>(fallback));
  }

  /// <summary>
  /// Adds a policy that cancels linked execution after completion.
  /// </summary>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> CancelAfterCompletion() {
    return Add(new CancelAfterCompletionPolicy<T1, T2>());
  }

  /// <summary>
  /// Adds an asynchronous policy created from a delegate.
  /// </summary>
  /// <param name="policy">Delegate implementing policy behavior.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Create(AsyncFunc<T1, IAsyncExecutor<T1, T2>, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return Add(new AsyncAnonymousPolicy<T1, T2>(policy));
  }

  /// <summary>
  /// Adds a policy to the builder pipeline.
  /// </summary>
  /// <param name="policy">Policy to add.</param>
  /// <returns>Current builder instance.</returns>
  public AsyncPolicyBuilder<T1, T2> Add(AsyncPolicy<T1, T2> policy) {
    _policies.Add(policy);
    return this;
  }

  [Pure]
  internal IAsyncExecutor<T1, T2> Apply(IAsyncExecutor<T1, T2> executor) {
    return _policies.Count == 0
      ? executor
      : _policies.Aggregate(executor, (current, policy) => current.Apply(policy));
  }

}