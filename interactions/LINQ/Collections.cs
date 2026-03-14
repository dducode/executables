using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.LINQ;

public static class Collections {

  [Pure]
  public static IExecutable<IEnumerable<T>, IEnumerable<T>> Where<T>(Validator<T> validator) {
    ExceptionsHelper.ThrowIfNull(validator, nameof(validator));
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Where(validator.IsValid));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, IEnumerable<T>> Where<T>(Func<T, bool> predicate) {
    ExceptionsHelper.ThrowIfNull(predicate, nameof(predicate));
    return Where(Validator.Create(predicate, string.Empty));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, IEnumerable<T>> Distinct<T>(IEqualityComparer<T> equalityComparer = null) {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Distinct(equalityComparer));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, IEnumerable<T>> Skip<T>(int skipCount) {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Skip(skipCount));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, IEnumerable<T>> Take<T>(int takeCount) {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Take(takeCount));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, T> Aggregate<T>(Func<T, T, T> accumulate) {
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Aggregate(accumulate));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, T2> Aggregate<T1, T2>(Func<T2> seed, Func<T2, T1, T2> accumulate) {
    ExceptionsHelper.ThrowIfNull(seed, nameof(seed));
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return Executable.Create((IEnumerable<T1> enumerable) => enumerable.Aggregate(seed(), accumulate));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T2>> Select<T1, T2>(Func<T1, T2> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return Executable.Create((IEnumerable<T1> enumerable) => enumerable.Select(selection));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T2>> SelectMany<T1, T2>(Func<T1, IEnumerable<T2>> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return Executable.Create((IEnumerable<T1> enumerable) => enumerable.SelectMany(selection));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, T> First<T>(Func<T, bool> predicate = null) {
    return predicate != null
      ? Executable.Create((IEnumerable<T> enumerable) => enumerable.First(predicate))
      : Executable.Create((IEnumerable<T> enumerable) => enumerable.First());
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, T> Single<T>() {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Single());
  }

}