using Interactions.Validation;

namespace Interactions.Policies;

public static partial class PolicyBuilderExtensions {

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

}