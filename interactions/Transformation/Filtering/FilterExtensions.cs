using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Handlers;
using Interactions.Extensions;
using Interactions.Handlers;
using Interactions.Validation;

namespace Interactions.Transformation.Filtering;

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

  [Pure]
  public static Handler<List<T>, List<T>> ListFilter<T>(this Handler<List<T>, List<T>> handler, Filter<T> incoming, Filter<T> outgoing) {
    return handler.Transform(Transformer.ToList<T>())
      .Filter(incoming, outgoing)
      .Transform(Transformer.ToList<T>().Inverse());
  }

  [Pure]
  public static Handler<List<T1>, T2> InputListFilter<T1, T2>(this Handler<List<T1>, T2> handler, Filter<T1> incoming) {
    return handler.InputTransform(Transformer.ToList<T1>())
      .InputFilter(incoming)
      .InputTransform(Transformer.ToList<T1>().Inverse());
  }

  [Pure]
  public static Handler<T1, List<T2>> OutputListFilter<T1, T2>(this Handler<T1, List<T2>> handler, Filter<T2> outgoing) {
    return handler.OutputTransform(Transformer.ToList<T2>().Inverse())
      .OutputFilter(outgoing)
      .OutputTransform(Transformer.ToList<T2>());
  }

  [Pure]
  public static Handler<T[], T[]> ArrayFilter<T>(this Handler<T[], T[]> handler, Filter<T> incoming, Filter<T> outgoing) {
    return handler.Transform(Transformer.ToArray<T>())
      .Filter(incoming, outgoing)
      .Transform(Transformer.ToArray<T>().Inverse());
  }

  [Pure]
  public static Handler<T1[], T2> InputArrayFilter<T1, T2>(this Handler<T1[], T2> handler, Filter<T1> incoming) {
    return handler.InputTransform(Transformer.ToArray<T1>())
      .InputFilter(incoming)
      .InputTransform(Transformer.ToArray<T1>().Inverse());
  }

  [Pure]
  public static Handler<T1, T2[]> OutputArrayFilter<T1, T2>(this Handler<T1, T2[]> handler, Filter<T2> outgoing) {
    return handler.OutputTransform(Transformer.ToArray<T2>().Inverse())
      .OutputFilter(outgoing)
      .OutputTransform(Transformer.ToArray<T2>());
  }

}