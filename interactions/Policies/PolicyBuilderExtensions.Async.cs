using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Fallbacks;
using Interactions.RetryRules;

namespace Interactions.Policies;

public static partial class PolicyBuilderExtensions {

  /// <summary>
  /// Creates an asynchronous retry policy from a delegate rule.
  /// </summary>
  /// <typeparam name="TEx">Exception type that can trigger retries.</typeparam>
  /// <param name="rule">
  /// Delegate that receives current failed-attempt count and exception instance,
  /// and returns <see langword="true"/> to continue retrying.
  /// </param>
  public static AsyncPolicyBuilder<T1, T2> Retry<T1, T2, TEx>(this AsyncPolicyBuilder<T1, T2> builder, AsyncFunc<int, TEx, bool> rule) where TEx : Exception {
    return builder.Retry(RetryRule.Create(rule));
  }

  /// <summary>
  /// Creates a policy that validates only invocation input.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  public static AsyncPolicyBuilder<T1, T2> ValidateInput<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, Validator<T1> inputValidator) {
    return builder.Validate(inputValidator, Validator.Identity<T2>());
  }

  /// <summary>
  /// Creates a policy that validates only invocation output.
  /// </summary>
  /// <param name="outputValidator">Validator for invocation result.</param>
  public static AsyncPolicyBuilder<T1, T2> ValidateOutput<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, Validator<T2> outputValidator) {
    return builder.Validate(Validator.Identity<T1>(), outputValidator);
  }

  /// <summary>
  /// Creates an input-validation policy from a predicate.
  /// </summary>
  /// <param name="inputValidator">Predicate that returns <see langword="true"/> for valid input.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  public static AsyncPolicyBuilder<T1, T2> ValidateInput<T1, T2>(
    this AsyncPolicyBuilder<T1, T2> builder,
    Func<T1, bool> inputValidator,
    string errorMessage) {
    return builder.ValidateInput(Validator.Create(inputValidator, errorMessage));
  }

  /// <summary>
  /// Creates an output-validation policy from a predicate.
  /// </summary>
  /// <param name="outputValidator">Predicate that returns <see langword="true"/> for valid output.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  public static AsyncPolicyBuilder<T1, T2> ValidateOutput<T1, T2>(
    this AsyncPolicyBuilder<T1, T2> builder,
    Func<T2, bool> outputValidator,
    string errorMessage) {
    return builder.ValidateOutput(Validator.Create(outputValidator, errorMessage));
  }

  /// <summary>
  /// Creates a guard policy from a predicate and denial message.
  /// </summary>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  public static AsyncPolicyBuilder<T1, T2> Guard<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, Func<bool> guard, string errorMessage) {
    return builder.Guard(Guards.Guard.Create(guard, errorMessage));
  }

  public static AsyncPolicyBuilder<T1, T2> Fallback<T1, T2, TEx>(this AsyncPolicyBuilder<T1, T2> builder, Func<T1, TEx, T2> fallback) where TEx : Exception {
    return builder.Fallback(FallbackHandler.Create(fallback));
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> Apply<T1, T2>(this AsyncPolicyBuilder<T1, T2> builder, AsyncFunc<T1, T2> func) {
    return builder.Apply(AsyncExecutable.Create(func));
  }

  [Pure]
  public static IAsyncExecutable<Unit, T> Apply<T>(this AsyncPolicyBuilder<Unit, T> builder, AsyncFunc<T> func) {
    return builder.Apply(AsyncExecutable.Create(func));
  }

  [Pure]
  public static IAsyncExecutable<T, Unit> Apply<T>(this AsyncPolicyBuilder<T, Unit> builder, AsyncAction<T> action) {
    return builder.Apply(AsyncExecutable.Create(action));
  }

  [Pure]
  public static IAsyncExecutable<Unit, Unit> Apply(this AsyncPolicyBuilder<Unit, Unit> builder, AsyncAction action) {
    return builder.Apply(AsyncExecutable.Create(action));
  }

}