using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Executables;

namespace Interactions.Filtering;

public static class Filter {

  [Pure]
  public static IFilter<T> Where<T>(Validator<T> validator) {
    ExceptionsHelper.ThrowIfNull(validator, nameof(validator));
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Where(validator.IsValid)).AsFilter();
  }

  [Pure]
  public static IFilter<T> Where<T>(Func<T, bool> predicate) {
    ExceptionsHelper.ThrowIfNull(predicate, nameof(predicate));
    return Where(Validator.Create(predicate, string.Empty));
  }

  [Pure]
  public static IFilter<T> Distinct<T>(IEqualityComparer<T> equalityComparer = null) {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Distinct(equalityComparer)).AsFilter();
  }

  [Pure]
  public static IFilter<T> Skip<T>(int skipCount) {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Skip(skipCount)).AsFilter();
  }

  [Pure]
  public static IFilter<T> Take<T>(int takeCount) {
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Take(takeCount)).AsFilter();
  }

}