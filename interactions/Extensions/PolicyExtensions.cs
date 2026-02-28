using System.Diagnostics.Contracts;
using Interactions.Analytics;
using Interactions.Core;
using Interactions.Guards;
using Interactions.Policies;
using Interactions.Validation;

namespace Interactions.Extensions;

public static partial class PolicyExtensions {

  /// <summary>
  /// Composes two policies so that <paramref name="other"/> wraps <paramref name="policy"/>.
  /// The last added policy runs first.
  /// </summary>
  /// <typeparam name="T1">Input value type.</typeparam>
  /// <typeparam name="T2">Result value type.</typeparam>
  /// <param name="policy">Base policy.</param>
  /// <param name="other">Wrapping policy added on top of base policy.</param>
  /// <returns>Composite policy.</returns>
  [Pure]
  public static Policy<T1, T2> Compose<T1, T2>(this Policy<T1, T2> policy, Policy<T1, T2> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new CompositePolicy<T1, T2>(policy, other);
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
  public static Policy<T1, T2> Validate<T1, T2>(this Policy<T1, T2> policy, Validator<T1> inputValidator, Validator<T2> outputValidator) {
    return policy.Compose(Policy<T1, T2>.Validate(inputValidator, outputValidator));
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
  public static Policy<T1, T2> ValidateInput<T1, T2>(this Policy<T1, T2> policy, Validator<T1> inputValidator) {
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
  public static Policy<T1, T2> ValidateOutput<T1, T2>(this Policy<T1, T2> policy, Validator<T2> outputValidator) {
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
  public static Policy<T1, T2> ValidateInput<T1, T2>(this Policy<T1, T2> policy, Func<T1, bool> inputValidator, string errorMessage) {
    return policy.ValidateInput(Validator.FromMethod(inputValidator, errorMessage));
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
  public static Policy<T1, T2> ValidateOutput<T1, T2>(this Policy<T1, T2> policy, Func<T2, bool> outputValidator, string errorMessage) {
    return policy.ValidateOutput(Validator.FromMethod(outputValidator, errorMessage));
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
  public static Policy<T1, T2> Metrics<T1, T2>(this Policy<T1, T2> policy, IMetrics<T1, T2> metrics, string tag = null) {
    return policy.Compose(Policy<T1, T2>.Metrics(metrics, tag));
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
  public static Policy<T1, T2> Guard<T1, T2>(this Policy<T1, T2> policy, Guard guard) {
    return policy.Compose(Policy<T1, T2>.Guard(guard));
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
  public static Policy<T1, T2> Guard<T1, T2>(this Policy<T1, T2> policy, Func<bool> guard, string errorMessage) {
    return policy.Guard(Guards.Guard.FromMethod(guard, errorMessage));
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
  public static Policy<T1, T2> Cache<T1, T2>(this Policy<T1, T2> policy, ICacheStorage<T1, T2> storage) {
    return policy.Compose(Policy<T1, T2>.Cache(storage));
  }

}