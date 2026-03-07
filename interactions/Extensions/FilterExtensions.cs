using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Transformation.Filtering;
using Interactions.Validation;

namespace Interactions.Extensions;

public static class FilterExtensions {

  [Pure]
  public static Filter<T> Where<T>(this Filter<T> filter, Validator<T> validator) {
    return filter.Compose(new ConditionalFilter<T>(validator));
  }

  [Pure]
  public static Filter<T> Where<T>(this Filter<T> filter, Func<T, bool> predicate) {
    return filter.Where(Validator.Create(predicate, string.Empty));
  }

  [Pure]
  public static Filter<T> Distinct<T>(this Filter<T> filter, IEqualityComparer<T> equalityComparer = null) {
    return filter.Compose(equalityComparer == null ? UniqueFilter<T>.Instance : new UniqueFilter<T>(equalityComparer));
  }

  [Pure]
  public static Filter<T> Skip<T>(this Filter<T> filter, int skipCount) {
    return filter.Compose(new Skipper<T>(skipCount));
  }

  [Pure]
  public static Filter<T> Take<T>(this Filter<T> filter, int takeCount) {
    return filter.Compose(new Taker<T>(takeCount));
  }

  [Pure]
  public static Filter<T> Compose<T>(this Filter<T> first, Filter<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeFilter<T>(first, second);
  }

}