using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Fallbacks;
using Interactions.Guards;
using Interactions.Policies;
using Interactions.Validation;

namespace Interactions;

/// <summary>
/// Factory methods for creating synchronous policies.
/// </summary>
public static class Policy {

  /// <summary>
  /// Creates a builder for policies with distinct input and output types.
  /// </summary>
  /// <returns>Policy builder.</returns>
  public static PolicyBuilder<T1, T2> Build<T1, T2>() {
    return new PolicyBuilder<T1, T2>();
  }

  /// <summary>
  /// Creates a builder for policies with identical input and output type.
  /// </summary>
  /// <returns>Policy builder.</returns>
  public static PolicyBuilder<T, T> Build<T>() {
    return new PolicyBuilder<T, T>();
  }

  /// <summary>
  /// Creates a no-op policy that only executes wrapped invocation.
  /// </summary>
  /// <returns>Policy instance that does not alter invocation behavior.</returns>
  [Pure]
  public static Policy<T1, T2> Identity<T1, T2>() {
    return IdentityPolicy<T1, T2>.Instance;
  }

  /// <summary>
  /// Creates a policy that validates input before invocation and output after invocation.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for both input and output.</returns>
  [Pure]
  public static Policy<T1, T2> Validate<T1, T2>(Validator<T1> inputValidator, Validator<T2> outputValidator) {
    ExceptionsHelper.ThrowIfNull(inputValidator, nameof(inputValidator));
    ExceptionsHelper.ThrowIfNull(outputValidator, nameof(outputValidator));
    return new ValidationPolicy<T1, T2>(inputValidator, outputValidator);
  }

  /// <summary>
  /// Creates a policy that validates only invocation input.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <returns>Validation policy for input with identity output validation.</returns>
  [Pure]
  public static Policy<T1, T2> ValidateInput<T1, T2>(Validator<T1> inputValidator) {
    return Validate(inputValidator, Validator.Identity<T2>());
  }

  /// <summary>
  /// Creates a policy that validates only invocation output.
  /// </summary>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for output with identity input validation.</returns>
  [Pure]
  public static Policy<T1, T2> ValidateOutput<T1, T2>(Validator<T2> outputValidator) {
    return Validate(Validator.Identity<T1>(), outputValidator);
  }

  /// <summary>
  /// Creates an input-validation policy from a predicate.
  /// </summary>
  /// <param name="inputValidator">Predicate that returns <see langword="true"/> for valid input.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Validation policy for invocation input.</returns>
  [Pure]
  public static Policy<T1, T2> ValidateInput<T1, T2>(Func<T1, bool> inputValidator, string errorMessage) {
    return ValidateInput<T1, T2>(Validator.Create(inputValidator, errorMessage));
  }

  /// <summary>
  /// Creates an output-validation policy from a predicate.
  /// </summary>
  /// <param name="outputValidator">Predicate that returns <see langword="true"/> for valid output.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Validation policy for invocation output.</returns>
  [Pure]
  public static Policy<T1, T2> ValidateOutput<T1, T2>(Func<T2, bool> outputValidator, string errorMessage) {
    return ValidateOutput<T1, T2>(Validator.Create(outputValidator, errorMessage));
  }

  /// <summary>
  /// Creates a policy that blocks invocation when the guard denies access.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Guard policy.</returns>
  [Pure]
  public static Policy<T1, T2> Guard<T1, T2>(Guard guard) {
    ExceptionsHelper.ThrowIfNull(guard, nameof(guard));
    return new GuardPolicy<T1, T2>(guard);
  }

  /// <summary>
  /// Creates a guard policy for identical input and output type.
  /// </summary>
  /// <param name="guard">Guard that controls whether invocation is allowed.</param>
  /// <returns>Guard policy.</returns>
  [Pure]
  public static Policy<T, T> Guard<T>(Guard guard) {
    return Guard<T, T>(guard);
  }

  /// <summary>
  /// Creates a guard policy from a predicate and denial message.
  /// </summary>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  /// <returns>Guard policy.</returns>
  [Pure]
  public static Policy<T1, T2> Guard<T1, T2>(Func<bool> guard, string errorMessage) {
    return Guard<T1, T2>(Guards.Guard.Create(guard, errorMessage));
  }

  /// <summary>
  /// Creates a guard policy for identical input and output type from a predicate.
  /// </summary>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  /// <returns>Guard policy.</returns>
  [Pure]
  public static Policy<T, T> Guard<T>(Func<bool> guard, string errorMessage) {
    return Guard<T, T>(guard, errorMessage);
  }

  /// <summary>
  /// Creates a policy that rejects reentrant execution.
  /// </summary>
  /// <returns>Prevent-reentrance policy.</returns>
  [Pure]
  public static Policy<T1, T2> PreventReentrance<T1, T2>() {
    return new PreventReentrancePolicy<T1, T2>();
  }

  /// <summary>
  /// Creates a policy that rejects reentrant execution for identical input and output type.
  /// </summary>
  /// <returns>Prevent-reentrance policy.</returns>
  [Pure]
  public static Policy<T, T> PreventReentrance<T>() {
    return PreventReentrance<T, T>();
  }

  /// <summary>
  /// Creates a fallback policy that handles exceptions using a fallback handler.
  /// </summary>
  /// <param name="fallback">Fallback handler invoked after a handled exception.</param>
  /// <returns>Fallback policy.</returns>
  [Pure]
  public static Policy<T1, T2> Fallback<T1, T2, TEx>(IFallbackHandler<T1, TEx, T2> fallback) where TEx : Exception {
    ExceptionsHelper.ThrowIfNull(fallback, nameof(fallback));
    return new FallbackPolicy<T1, T2, TEx>(fallback);
  }

  /// <summary>
  /// Creates a fallback policy from a fallback delegate.
  /// </summary>
  /// <param name="fallback">Delegate that converts input and exception into a fallback result.</param>
  /// <returns>Fallback policy.</returns>
  [Pure]
  public static Policy<T1, T2> Fallback<T1, T2, TEx>(Func<T1, TEx, T2> fallback) where TEx : Exception {
    return Fallback(FallbackHandler.Create(fallback));
  }

  /// <summary>
  /// Creates a policy from a delegate.
  /// </summary>
  /// <param name="policy">Delegate implementing policy behavior.</param>
  /// <returns>Policy wrapping <paramref name="policy"/>.</returns>
  [Pure]
  public static Policy<T1, T2> Create<T1, T2>(Func<T1, IExecutor<T1, T2>, T2> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new AnonymousPolicy<T1, T2>(policy);
  }

  /// <summary>
  /// Creates a policy that executes work on the thread pool.
  /// </summary>
  /// <returns>Thread-pool policy.</returns>
  [Pure]
  public static Policy<T, Unit> OnThreadPool<T>() {
    return new ThreadPoolPolicy<T>();
  }

}