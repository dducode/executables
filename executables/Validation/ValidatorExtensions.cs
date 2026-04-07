using System.Diagnostics.Contracts;
using Executables.Core.Validation;
using Executables.Internal;

namespace Executables.Validation;

/// <summary>
/// Extension methods for composing validators.
/// </summary>
public static class ValidatorExtensions {

  /// <summary>
  /// Combines two validators with logical AND semantics.
  /// </summary>
  /// <typeparam name="T">Validated value type.</typeparam>
  /// <param name="first">First validator in composition chain.</param>
  /// <param name="second">Second validator in composition chain.</param>
  /// <returns>Validator that succeeds only when both validators succeed.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static Validator<T> And<T>(this Validator<T> first, Validator<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AndValidator<T>(first, second);
  }

  /// <summary>
  /// Combines two validators with logical OR semantics.
  /// </summary>
  /// <typeparam name="T">Validated value type.</typeparam>
  /// <param name="first">First validator in composition chain.</param>
  /// <param name="second">Second validator in composition chain.</param>
  /// <returns>Validator that succeeds when at least one validator succeeds.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static Validator<T> Or<T>(this Validator<T> first, Validator<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new OrValidator<T>(first, second);
  }

  /// <summary>
  /// Replaces the validation error message produced by a validator.
  /// </summary>
  /// <typeparam name="T">Validated value type.</typeparam>
  /// <param name="validator">Validator whose error message should be replaced.</param>
  /// <param name="message">Replacement error message.</param>
  /// <returns>Validator that preserves validation logic and overrides the error message.</returns>
  /// <exception cref="ArgumentException"><paramref name="message"/> is empty.</exception>
  [Pure]
  public static Validator<T> OverrideMessage<T>(this Validator<T> validator, string message) {
    ExceptionsHelper.ThrowIfNullOrEmpty(message, nameof(message));
    return new OverrideMessageValidator<T>(validator, message);
  }

}