using System.Diagnostics.Contracts;
using Interactions.Core;

namespace Interactions.Transformation;

public static class TransformersExtensions {

  [Pure]
  public static SymmetricTransformer<T2, T1> Inverse<T1, T2>(this SymmetricTransformer<T1, T2> transformer) {
    return new InvertedTransformer<T1, T2>(transformer);
  }

  [Pure]
  public static Transformer<T1, T3> Compose<T1, T2, T3>(this Transformer<T1, T2> first, Transformer<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeTransformer<T1, T2, T3>(first, second);
  }

  [Pure]
  public static SymmetricTransformer<T1, T3> Compose<T1, T2, T3>(this SymmetricTransformer<T1, T2> first, SymmetricTransformer<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeSymmetricTransformer<T1, T2, T3>(first, second);
  }

  [Pure]
  public static Transformer<IEnumerable<T1>, IEnumerable<T3>> Select<T1, T2, T3>(
    this Transformer<IEnumerable<T1>, IEnumerable<T2>> transformer,
    Func<T2, T3> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return transformer.Compose(new Selector<T2, T3>(selection));
  }

  [Pure]
  public static Transformer<IEnumerable<T1>, IEnumerable<T3>> SelectMany<T1, T2, T3>(
    this Transformer<IEnumerable<T1>, IEnumerable<T2>> transformer,
    Func<T2, IEnumerable<T3>> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return transformer.Compose(new ManySelector<T2, T3>(selection));
  }

}