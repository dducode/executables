using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Executables;

namespace Interactions.Transformation.Filtering;

public static class FilterExtensions {

  [Pure]
  public static Filter<T> Compose<T>(this Filter<T> first, Filter<T> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeFilter<T>(first, second);
  }

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
  public static IExecutable<List<T>, List<T>> ListFilter<T>(this IExecutable<List<T>, List<T>> handler, Filter<T> incoming, Filter<T> outgoing) {
    return handler.Map(Transformer.ToList<T>())
      .Filter(incoming, outgoing)
      .Map(Transformer.ToList<T>().Inverse());
  }

  [Pure]
  public static IExecutable<List<T1>, T2> InputListFilter<T1, T2>(this IExecutable<List<T1>, T2> handler, Filter<T1> incoming) {
    return handler.InMap(Transformer.ToList<T1>())
      .InFilter(incoming)
      .InMap(Transformer.ToList<T1>().Inverse());
  }

  [Pure]
  public static IExecutable<T1, List<T2>> OutputListFilter<T1, T2>(this IExecutable<T1, List<T2>> handler, Filter<T2> outgoing) {
    return handler.OutMap(Transformer.ToList<T2>().Inverse())
      .OutFilter(outgoing)
      .OutMap(Transformer.ToList<T2>());
  }

  [Pure]
  public static IExecutable<T[], T[]> ArrayFilter<T>(this IExecutable<T[], T[]> handler, Filter<T> incoming, Filter<T> outgoing) {
    return handler.Map(Transformer.ToArray<T>())
      .Filter(incoming, outgoing)
      .Map(Transformer.ToArray<T>().Inverse());
  }

  [Pure]
  public static IExecutable<T1[], T2> InputArrayFilter<T1, T2>(this IExecutable<T1[], T2> handler, Filter<T1> incoming) {
    return handler.InMap(Transformer.ToArray<T1>())
      .InFilter(incoming)
      .InMap(Transformer.ToArray<T1>().Inverse());
  }

  [Pure]
  public static IExecutable<T1, T2[]> OutputArrayFilter<T1, T2>(this IExecutable<T1, T2[]> handler, Filter<T2> outgoing) {
    return handler.OutMap(Transformer.ToArray<T2>().Inverse())
      .OutFilter(outgoing)
      .OutMap(Transformer.ToArray<T2>());
  }

}