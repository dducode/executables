using Interactions.Core;
using Interactions.Fallbacks;

namespace Interactions.Policies;

public static partial class PolicyBuilderExtensions {

  /// <summary>
  /// Creates a policy that validates only invocation input.
  /// </summary>
  /// <param name="inputValidator">Validator for invocation input.</param>
  /// <returns>Validation policy for input with identity output validation.</returns>
  public static PolicyBuilder<T1, T2> ValidateInput<T1, T2>(this PolicyBuilder<T1, T2> builder, Validator<T1> inputValidator) {
    return builder.Validate(inputValidator, Validator.Identity<T2>());
  }

  /// <summary>
  /// Creates a policy that validates only invocation output.
  /// </summary>
  /// <param name="outputValidator">Validator for invocation result.</param>
  /// <returns>Validation policy for output with identity input validation.</returns>
  public static PolicyBuilder<T1, T2> ValidateOutput<T1, T2>(this PolicyBuilder<T1, T2> builder, Validator<T2> outputValidator) {
    return builder.Validate(Validator.Identity<T1>(), outputValidator);
  }

  /// <summary>
  /// Creates an input-validation policy from a predicate.
  /// </summary>
  /// <param name="inputValidator">Predicate that returns <see langword="true"/> for valid input.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Validation policy for invocation input.</returns>
  public static PolicyBuilder<T1, T2> ValidateInput<T1, T2>(this PolicyBuilder<T1, T2> builder, Func<T1, bool> inputValidator, string errorMessage) {
    return builder.Validate(Validator.Create(inputValidator, errorMessage), Validator.Identity<T2>());
  }

  /// <summary>
  /// Creates an output-validation policy from a predicate.
  /// </summary>
  /// <param name="outputValidator">Predicate that returns <see langword="true"/> for valid output.</param>
  /// <param name="errorMessage">Message used when validation fails.</param>
  /// <returns>Validation policy for invocation output.</returns>
  public static PolicyBuilder<T1, T2> ValidateOutput<T1, T2>(this PolicyBuilder<T1, T2> builder, Func<T2, bool> outputValidator, string errorMessage) {
    return builder.Validate(Validator.Identity<T1>(), Validator.Create(outputValidator, errorMessage));
  }

  /// <summary>
  /// Creates a guard policy from a predicate and denial message.
  /// </summary>
  /// <param name="guard">Predicate that returns <see langword="true"/> when invocation is allowed.</param>
  /// <param name="errorMessage">Message used when guard denies access.</param>
  /// <returns>Guard policy.</returns>
  public static PolicyBuilder<T1, T2> Guard<T1, T2>(this PolicyBuilder<T1, T2> builder, Func<bool> guard, string errorMessage) {
    return builder.Guard(Guards.Guard.Create(guard, errorMessage));
  }

  public static PolicyBuilder<T1, T2> Fallback<T1, T2, TEx>(this PolicyBuilder<T1, T2> builder, Func<T1, TEx, T2> fallback) where TEx : Exception {
    return builder.Fallback(FallbackHandler.Create(fallback));
  }

  public static PolicyBuilder<T, Unit> OnThreadPool<T>(this PolicyBuilder<T, Unit> builder) {
    return builder.Add(new ThreadPoolPolicy<T>());
  }

}