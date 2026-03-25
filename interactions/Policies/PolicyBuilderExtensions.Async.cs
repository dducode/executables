using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Fallbacks;
using Interactions.RetryRules;

namespace Interactions.Policies;

public static partial class PolicyBuilderExtensions {

  /// <summary>
  /// Creates an asynchronous retry policy from a delegate rule.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="rule">
  /// Delegate that receives current failed-attempt count and exception instance,
  /// and returns <see langword="true"/> to continue retrying.
  /// </param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> Retry<T1, T2, TEx>(this AsyncPolicyBuilder<T1, T2> builder, AsyncFunc<int, TEx, bool> rule) where TEx : Exception {
    return builder.Retry(RetryRule.Create(rule));
  }

  /// <summary>
  /// Creates a policy that validates only invocation input.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> ValidateInput<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, Validator<T1> inputValidator) {
    return builder.Validate(inputValidator, Validator.Identity<T2>());
  }

  /// <summary>
  /// Creates a policy that validates only invocation output.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> ValidateOutput<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, Validator<T2> outputValidator) {
    return builder.Validate(Validator.Identity<T1>(), outputValidator);
  }

  /// <summary>
  /// Creates an input-validation policy from a predicate.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="inputValidator">Predicate that returns <see langword="true"/> for valid input.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> ValidateInput<T1, T2>(
    this AsyncPolicyBuilder<T1, T2> builder,
    Func<T1, bool> inputValidator,
    string errorMessage) {
    return builder.ValidateInput(Validator.Create(inputValidator, errorMessage));
  }

  /// <summary>
  /// Creates an output-validation policy from a predicate.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="outputValidator">Predicate that returns <see langword="true"/> for valid output.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> ValidateOutput<T1, T2>(
    this AsyncPolicyBuilder<T1, T2> builder,
    Func<T2, bool> outputValidator,
    string errorMessage) {
    return builder.ValidateOutput(Validator.Create(outputValidator, errorMessage));
  }

  /// <summary>
  /// Creates a guard policy from a predicate and denial message.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> Guard<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, Func<bool> guard, string errorMessage) {
    return builder.Guard(Guards.Guard.Create(guard, errorMessage));
  }

  /// <summary>
  /// Creates a fallback policy from a delegate.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="fallback">Delegate that converts input and exception into a fallback result.</param>
  /// <returns>Current builder instance.</returns>
  public static AsyncPolicyBuilder<T1, T2> Fallback<T1, T2, TEx>(this AsyncPolicyBuilder<T1, T2> builder, Func<T1, TEx, T2> fallback) where TEx : Exception {
    return builder.Fallback(FallbackHandler.Create(fallback));
  }

  /// <summary>
  /// Applies configured policies to an asynchronous delegate converted into executable.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="func">Delegate to wrap with configured policies.</param>
  /// <returns>Asynchronous executable wrapped with configured policies.</returns>
  [Pure]
  public static IAsyncExecutable<T1, T2> Apply<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, AsyncFunc<T1, T2> func) {
    return builder.Apply(AsyncExecutable.Create(func));
  }

  /// <summary>
  /// Applies configured policies to a parameterless asynchronous delegate converted into executable.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="func">Delegate to wrap with configured policies.</param>
  /// <returns>Asynchronous executable wrapped with configured policies.</returns>
  [Pure]
  public static IAsyncExecutable<Unit, T> Apply<T>(this AsyncPolicyBuilder<Unit, T> builder, AsyncFunc<T> func) {
    return builder.Apply(AsyncExecutable.Create(func));
  }

  /// <summary>
  /// Applies configured policies to an asynchronous action converted into executable.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="action">Action to wrap with configured policies.</param>
  /// <returns>Asynchronous executable wrapped with configured policies.</returns>
  [Pure]
  public static IAsyncExecutable<T, Unit> Apply<T>(this AsyncPolicyBuilder<T, Unit> builder, AsyncAction<T> action) {
    return builder.Apply(AsyncExecutable.Create(action));
  }

  /// <summary>
  /// Applies configured policies to a parameterless asynchronous action converted into executable.
  /// </summary>
  /// <param name="builder">Policy builder.</param>
  /// <param name="action">Action to wrap with configured policies.</param>
  /// <returns>Asynchronous executable wrapped with configured policies.</returns>
  [Pure]
  public static IAsyncExecutable<Unit, Unit> Apply(this AsyncPolicyBuilder<Unit, Unit> builder, AsyncAction action) {
    return builder.Apply(AsyncExecutable.Create(action));
  }

}