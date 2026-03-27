using System.Diagnostics.Contracts;
using Interactions.Internal;
using Interactions.Validation;

namespace Interactions.Linq;

public static class CollectionsExtensions {

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> Where<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, Validator<T2> validator) {
    ExceptionsHelper.ThrowIfNull(validator, nameof(validator));
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Where(validator.IsValid)));
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> Where<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, Func<T2, bool> predicate) {
    return executable.Where(Validator.Create(predicate, string.Empty));
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> Distinct<T1, T2>(
    this IExecutable<T1, IEnumerable<T2>> executable,
    IEqualityComparer<T2> equalityComparer = null) {
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Distinct(equalityComparer)));
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> Skip<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, int skipCount) {
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Skip(skipCount)));
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> Take<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, int takeCount) {
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Take(takeCount)));
  }

  [Pure]
  public static IExecutable<T1, T2> Aggregate<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, Func<T2, T2, T2> accumulate) {
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Aggregate(accumulate)));
  }

  [Pure]
  public static IExecutable<T1, T3> Aggregate<T1, T2, T3>(this IExecutable<T1, IEnumerable<T2>> executable, Func<T3> seed, Func<T3, T2, T3> accumulate) {
    ExceptionsHelper.ThrowIfNull(seed, nameof(seed));
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Aggregate(seed(), accumulate)));
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T3>> Select<T1, T2, T3>(this IExecutable<T1, IEnumerable<T2>> executable, Func<T2, T3> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Select(selection)));
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T3>> SelectMany<T1, T2, T3>(this IExecutable<T1, IEnumerable<T2>> executable, Func<T2, IEnumerable<T3>> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.SelectMany(selection)));
  }

  [Pure]
  public static IExecutable<T1, T2> First<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, Func<T2, bool> predicate = null) {
    return executable.Then(predicate != null
      ? Executable.Create((IEnumerable<T2> enumerable) => enumerable.First(predicate))
      : Executable.Create((IEnumerable<T2> enumerable) => enumerable.First()));
  }

  [Pure]
  public static IExecutable<T1, T2> Single<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable) {
    return executable.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Single()));
  }

}