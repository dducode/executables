using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Interactions.Core.Validation;
using Interactions.Internal;

namespace Interactions.Validation;

/// <summary>
/// Factory methods and predefined instances for validators.
/// </summary>
public static class Validator {

  /// <summary>
  /// Gets a validator that accepts only zero.
  /// </summary>
  public static Validator<int> ZeroEqual { get; } = Equal(0);

  /// <summary>
  /// Gets a validator that rejects zero.
  /// </summary>
  public static Validator<int> ZeroNotEqual { get; } = NotEqual(0);

  /// <summary>
  /// Gets a validator that accepts values greater than zero.
  /// </summary>
  public static Validator<int> MoreThanZero { get; } = MoreThan(0);

  /// <summary>
  /// Gets a validator that accepts values less than or equal to zero.
  /// </summary>
  public static Validator<int> LessThanZeroOrEqual { get; } = LessThanOrEqual(0);

  /// <summary>
  /// Gets a validator that rejects <see langword="null"/> and empty strings.
  /// </summary>
  public static Validator<string> NotEmptyString { get; } = NotNull<string>().And(StringLength(MoreThan(0)))
    .OverrideMessage("String cannot be null or empty");

  /// <summary>
  /// Returns a validator that accepts every value.
  /// </summary>
  /// <returns>Identity validator.</returns>
  [Pure]
  public static Validator<T> Identity<T>() {
    return IdentityValidator<T>.Instance;
  }

  /// <summary>
  /// Creates a validator that negates another validator.
  /// </summary>
  /// <param name="other">Validator to negate.</param>
  /// <returns>Negated validator.</returns>
  [Pure]
  public static Validator<T> Not<T>(Validator<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new NotValidator<T>(other);
  }

  /// <summary>
  /// Returns a validator that rejects <see langword="null"/> references.
  /// </summary>
  /// <returns>Not-null validator.</returns>
  [Pure]
  public static Validator<T> NotNull<T>() where T : class {
    return NotNullValidator<T>.Instance;
  }

  /// <summary>
  /// Creates a validator that checks equality with an expected value.
  /// </summary>
  /// <param name="expected">Expected value.</param>
  /// <param name="comparer">Optional equality comparer.</param>
  /// <returns>Equality validator.</returns>
  [Pure]
  public static Validator<T> Equal<T>(T expected, IEqualityComparer<T> comparer = null) {
    return new EqualityValidator<T>(expected, comparer ?? EqualityComparer<T>.Default);
  }

  /// <summary>
  /// Creates a validator that checks inequality with an expected value.
  /// </summary>
  /// <param name="expected">Disallowed value.</param>
  /// <param name="comparer">Optional equality comparer.</param>
  /// <returns>Inequality validator.</returns>
  [Pure]
  public static Validator<T> NotEqual<T>(T expected, IEqualityComparer<T> comparer = null) {
    return Not(Equal(expected, comparer));
  }

  /// <summary>
  /// Creates a validator that checks whether a value is greater than the specified value.
  /// </summary>
  /// <param name="value">Lower exclusive bound.</param>
  /// <param name="comparer">Optional comparer.</param>
  /// <returns>Greater-than validator.</returns>
  [Pure]
  public static Validator<T> MoreThan<T>(T value, IComparer<T> comparer = null) {
    return new MoreThanValidator<T>(value, comparer ?? Comparer<T>.Default);
  }

  /// <summary>
  /// Creates a validator that checks whether a value is less than the specified value.
  /// </summary>
  /// <param name="value">Upper exclusive bound.</param>
  /// <param name="comparer">Optional comparer.</param>
  /// <returns>Less-than validator.</returns>
  [Pure]
  public static Validator<T> LessThan<T>(T value, IComparer<T> comparer = null) {
    return new LessThanValidator<T>(value, comparer ?? Comparer<T>.Default);
  }

  /// <summary>
  /// Creates a validator that checks whether a value is greater than or equal to the specified value.
  /// </summary>
  /// <param name="value">Lower inclusive bound.</param>
  /// <param name="comparer">Optional comparer.</param>
  /// <returns>Greater-than-or-equal validator.</returns>
  [Pure]
  public static Validator<T> MoreThanOrEqual<T>(T value, IComparer<T> comparer = null) {
    return new MoreThanOrEqualValidator<T>(value, comparer ?? Comparer<T>.Default);
  }

  /// <summary>
  /// Creates a validator that checks whether a value is less than or equal to the specified value.
  /// </summary>
  /// <param name="value">Upper inclusive bound.</param>
  /// <param name="comparer">Optional comparer.</param>
  /// <returns>Less-than-or-equal validator.</returns>
  [Pure]
  public static Validator<T> LessThanOrEqual<T>(T value, IComparer<T> comparer = null) {
    return new LessThanOrEqualValidator<T>(value, comparer ?? Comparer<T>.Default);
  }

  /// <summary>
  /// Creates a validator that checks whether a value belongs to the specified range.
  /// </summary>
  /// <param name="min">Lower bound.</param>
  /// <param name="max">Upper bound.</param>
  /// <param name="rightInclusive"><see langword="true"/> to include the right bound; otherwise it is exclusive.</param>
  /// <param name="comparer">Optional comparer.</param>
  /// <returns>Range validator.</returns>
  [Pure]
  public static Validator<T> InRange<T>(T min, T max, bool rightInclusive = false, IComparer<T> comparer = null) {
    return InRangeCore(min, max, rightInclusive, comparer)
      .OverrideMessage($"Value must be in range [{min}..{max}{(rightInclusive ? "]" : ")")}");
  }

  /// <summary>
  /// Creates a validator that checks whether a value lies outside the specified range.
  /// </summary>
  /// <param name="min">Lower bound.</param>
  /// <param name="max">Upper bound.</param>
  /// <param name="rightInclusive"><see langword="true"/> to include the right bound; otherwise it is exclusive.</param>
  /// <param name="comparer">Optional comparer.</param>
  /// <returns>Out-of-range validator.</returns>
  [Pure]
  public static Validator<T> OutRange<T>(T min, T max, bool rightInclusive = false, IComparer<T> comparer = null) {
    return Not(InRangeCore(min, max, rightInclusive, comparer))
      .OverrideMessage($"Value must be outside of range [{min}..{max}{(rightInclusive ? "]" : ")")}");
  }

  /// <summary>
  /// Creates a validator for string length.
  /// </summary>
  /// <param name="lengthValidator">Validator applied to string length.</param>
  /// <returns>String-length validator.</returns>
  [Pure]
  public static Validator<string> StringLength(Validator<int> lengthValidator) {
    ExceptionsHelper.ThrowIfNull(lengthValidator, nameof(lengthValidator));
    return new StringLengthValidator(lengthValidator);
  }

  /// <summary>
  /// Creates a validator for collection item count.
  /// </summary>
  /// <param name="validator">Validator applied to collection count.</param>
  /// <returns>Collection-count validator.</returns>
  [Pure]
  public static Validator<ICollection<T>> CollectionCount<T>(Validator<int> validator) {
    ExceptionsHelper.ThrowIfNull(validator, nameof(validator));
    return new CollectionCountValidator<T>(validator);
  }

  /// <summary>
  /// Returns a validator that rejects empty collections.
  /// </summary>
  /// <returns>Not-empty collection validator.</returns>
  [Pure]
  public static Validator<ICollection<T>> NotEmptyCollection<T>() {
    return CollectionCountValidator<T>.NotEmptyCollection;
  }

  /// <summary>
  /// Creates a validator that requires all sequence items to satisfy the provided validator.
  /// </summary>
  /// <param name="itemValidator">Validator applied to each item.</param>
  /// <returns>All-items validator.</returns>
  [Pure]
  public static Validator<IEnumerable<T>> All<T>(Validator<T> itemValidator) {
    ExceptionsHelper.ThrowIfNull(itemValidator, nameof(itemValidator));
    return new AllValidator<T>(itemValidator);
  }

  /// <summary>
  /// Creates a validator that requires at least one sequence item to satisfy the provided validator.
  /// </summary>
  /// <param name="itemValidator">Validator applied to each item.</param>
  /// <returns>Any-item validator.</returns>
  [Pure]
  public static Validator<IEnumerable<T>> Any<T>(Validator<T> itemValidator) {
    ExceptionsHelper.ThrowIfNull(itemValidator, nameof(itemValidator));
    return new AnyValidator<T>(itemValidator);
  }

  /// <summary>
  /// Creates a validator that matches strings against a regular expression.
  /// </summary>
  /// <param name="pattern">Regular expression pattern.</param>
  /// <param name="options">Regular expression options.</param>
  /// <returns>Regex validator.</returns>
  [Pure]
  public static Validator<string> Match(string pattern, RegexOptions options = RegexOptions.None) {
    ExceptionsHelper.ThrowIfNullOrEmpty(pattern, nameof(pattern));
    return new RegexValidator(pattern, options);
  }

  /// <summary>
  /// Creates a validator that checks whether a value is of the specified runtime type.
  /// </summary>
  /// <returns>Type validator.</returns>
  [Pure]
  public static Validator<object> Is<TExpected>() {
    return TypeValidator<TExpected>.Instance;
  }

  /// <summary>
  /// Creates a validator from a predicate and an error message.
  /// </summary>
  /// <param name="validation">Predicate used for validation.</param>
  /// <param name="errorMessage">Error message returned on validation failure.</param>
  /// <returns>Validator wrapping <paramref name="validation"/>.</returns>
  [Pure]
  public static Validator<T> Create<T>(Func<T, bool> validation, string errorMessage) {
    ExceptionsHelper.ThrowIfNull(validation, nameof(validation));
    ExceptionsHelper.ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage));
    return new AnonymousValidator<T>(validation, errorMessage);
  }

  private static Validator<T> InRangeCore<T>(T min, T max, bool rightInclusive = false, IComparer<T> comparer = null) {
    return MoreThanOrEqual(min, comparer).And(rightInclusive ? LessThanOrEqual(max, comparer) : LessThan(max, comparer));
  }

}