using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Transformation.Filtering;

public static class Filter {

  [Pure]
  public static Filter<T> Identity<T>() {
    return IdentityFilter<T>.Instance;
  }

  [Pure]
  public static Filter<T> Where<T>(Validator<T> validator) {
    ExceptionsHelper.ThrowIfNull(validator, nameof(validator));
    return new ConditionalFilter<T>(validator);
  }

  [Pure]
  public static Filter<T> Where<T>(Func<T, bool> predicate) {
    ExceptionsHelper.ThrowIfNull(predicate, nameof(predicate));
    return Where(Validator.Create(predicate, string.Empty));
  }

  [Pure]
  public static Filter<T> Distinct<T>(IEqualityComparer<T> equalityComparer = null) {
    return equalityComparer == null ? UniqueFilter<T>.Instance : new UniqueFilter<T>(equalityComparer);
  }

  [Pure]
  public static Filter<T> Skip<T>(int skipCount) {
    return new Skipper<T>(skipCount);
  }

  [Pure]
  public static Filter<T> Take<T>(int takeCount) {
    return new Taker<T>(takeCount);
  }

  [Pure]
  public static Filter<T> Create<T>(Func<IEnumerable<T>, IEnumerable<T>> filtration) {
    ExceptionsHelper.ThrowIfNull(filtration, nameof(filtration));
    return new AnonymousFilter<T>(filtration);
  }

}