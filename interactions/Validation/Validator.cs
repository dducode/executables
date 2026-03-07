using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Interactions.Core.Internal;
using Interactions.Core.Providers;
using Interactions.Core.Resolvers;

namespace Interactions.Validation;

public abstract class Validator<T> {

  public abstract string ErrorMessage { get; }

  public abstract bool IsValid(T value);

}

public static class Validator {

  [Pure] public static Validator<int> ZeroEqual { get; } = Equal(0);
  [Pure] public static Validator<int> ZeroNotEqual { get; } = NotEqual(0);

  [Pure]
  public static Validator<string> NotEmptyString { get; } = NotNull<string>().And(StringLength(MoreThan(0)))
    .OverrideMessage("String cannot be null or empty");

  [Pure]
  public static Validator<T> Identity<T>() {
    return IdentityValidator<T>.Instance;
  }

  [Pure]
  public static Validator<T> Dynamic<T>(IProvider<Validator<T>> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new DynamicValidator<T>(provider);
  }

  [Pure]
  public static Validator<T> Dynamic<T>(Func<Validator<T>> provider) {
    return Dynamic(Provider.Create(provider));
  }

  [Pure]
  public static Validator<T> Lazy<T>(IResolver<Validator<T>> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new LazyValidator<T>(resolver);
  }

  [Pure]
  public static Validator<T> Lazy<T>(Func<Validator<T>> provider) {
    return Lazy(Resolver.Create(provider));
  }

  [Pure]
  public static Validator<T> Not<T>(Validator<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new NotValidator<T>(other);
  }

  [Pure]
  public static Validator<T> NotNull<T>() where T : class {
    return NotNullValidator<T>.Instance;
  }

  [Pure]
  public static Validator<T> Equal<T>(T expected, IEqualityComparer<T> comparer = null) {
    return new EqualityValidator<T>(expected, comparer ?? EqualityComparer<T>.Default);
  }

  [Pure]
  public static Validator<T> NotEqual<T>(T expected, IEqualityComparer<T> comparer = null) {
    return Not(Equal(expected, comparer));
  }

  [Pure]
  public static Validator<T> MoreThan<T>(T value, IComparer<T> comparer = null) {
    return new MoreThanValidator<T>(value, comparer ?? Comparer<T>.Default);
  }

  [Pure]
  public static Validator<T> LessThan<T>(T value, IComparer<T> comparer = null) {
    return new LessThanValidator<T>(value, comparer ?? Comparer<T>.Default);
  }

  [Pure]
  public static Validator<T> MoreThanOrEqual<T>(T value, IComparer<T> comparer = null, IEqualityComparer<T> equalityComparer = null) {
    return MoreThan(value, comparer).Or(Equal(value, equalityComparer));
  }

  [Pure]
  public static Validator<T> LessThanOrEqual<T>(T value, IComparer<T> comparer = null, IEqualityComparer<T> equalityComparer = null) {
    return LessThan(value, comparer).Or(Equal(value, equalityComparer));
  }

  [Pure]
  public static Validator<T> InRange<T>(T min, T max, bool rightInclusive = false, IComparer<T> c = null, IEqualityComparer<T> ec = null) {
    return InRangeCore(min, max, rightInclusive, c, ec)
      .OverrideMessage($"Value must be in range [{min}..{max}{(rightInclusive ? "]" : ")")}");
  }

  [Pure]
  public static Validator<T> OutRange<T>(T min, T max, bool rightInclusive = false, IComparer<T> c = null, IEqualityComparer<T> ec = null) {
    return Not(InRangeCore(min, max, rightInclusive, c, ec))
      .OverrideMessage($"Value must be outside of range [{min}..{max}{(rightInclusive ? "]" : ")")}");
  }

  [Pure]
  public static Validator<string> StringLength(Validator<int> lengthValidator) {
    ExceptionsHelper.ThrowIfNull(lengthValidator, nameof(lengthValidator));
    return new StringLengthValidator(lengthValidator);
  }

  [Pure]
  public static Validator<ICollection<T>> CollectionCount<T>(Validator<int> validator) {
    ExceptionsHelper.ThrowIfNull(validator, nameof(validator));
    return new CollectionCountValidator<T>(validator);
  }

  [Pure]
  public static Validator<ICollection<T>> NotEmptyCollection<T>() {
    return CollectionCountValidator<T>.NotEmptyCollection;
  }

  [Pure]
  public static Validator<IEnumerable<T>> All<T>(Validator<T> itemValidator) {
    ExceptionsHelper.ThrowIfNull(itemValidator, nameof(itemValidator));
    return new AllValidator<T>(itemValidator);
  }

  [Pure]
  public static Validator<IEnumerable<T>> Any<T>(Validator<T> itemValidator) {
    ExceptionsHelper.ThrowIfNull(itemValidator, nameof(itemValidator));
    return new AnyValidator<T>(itemValidator);
  }

  [Pure]
  public static Validator<string> Match(string pattern, RegexOptions options = RegexOptions.None) {
    ExceptionsHelper.ThrowIfNullOrEmpty(pattern, nameof(pattern));
    return new RegexValidator(pattern, options);
  }

  [Pure]
  public static Validator<object> Is<TExpected>() {
    return TypeValidator<TExpected>.Instance;
  }

  [Pure]
  public static Validator<T> Create<T>(Func<T, bool> validation, string errorMessage) {
    ExceptionsHelper.ThrowIfNull(validation, nameof(validation));
    ExceptionsHelper.ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage));
    return new AnonymousValidator<T>(validation, errorMessage);
  }

  [Pure]
  private static Validator<T> InRangeCore<T>(T min, T max, bool rightInclusive = false, IComparer<T> c = null, IEqualityComparer<T> ec = null) {
    return MoreThan(min, c).Or(Equal(min, ec)).And(rightInclusive ? LessThan(max, c).Or(Equal(max, ec)) : LessThan(max, c));
  }

}