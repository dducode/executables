using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Core.Internal;
using Interactions.Transformation;
using Interactions.Transformation.Filtering;
using Interactions.Transformation.Parsing;

namespace Interactions.Handlers;

public static partial class TransformHandlersExtensions {

  [Pure]
  public static AsyncHandler<T2, T2> Transform<T1, T2>(this AsyncHandler<T1, T1> handler, SymmetricTransformer<T2, T1> transformer) {
    ExceptionsHelper.ThrowIfNull(transformer, nameof(transformer));
    return new AsyncSymmetricTransformHandler<T2, T1>(transformer, handler);
  }

  [Pure]
  public static AsyncHandler<string, string> Parse<T1, T2>(this AsyncHandler<T1, T2> handler, Parser<T1> parser) {
    return handler.Transform(parser, ToStringTransformer<T2>.Instance);
  }

  [Pure]
  public static AsyncHandler<string, T2> InputParse<T1, T2>(this AsyncHandler<T1, T2> handler, Parser<T1> parser) {
    return handler.InputTransform(parser);
  }

  [Pure]
  public static AsyncHandler<IEnumerable<T1>, IEnumerable<T2>> Filter<T1, T2>(
    this AsyncHandler<IEnumerable<T1>, IEnumerable<T2>> handler, Filter<T1> incoming, Filter<T2> outgoing) {
    return handler.Transform(incoming, outgoing);
  }

  [Pure]
  public static AsyncHandler<IEnumerable<T1>, T2> InputFilter<T1, T2>(this AsyncHandler<IEnumerable<T1>, T2> handler, Filter<T1> filter) {
    return handler.InputTransform(filter);
  }

  [Pure]
  public static AsyncHandler<T1, IEnumerable<T2>> OutputFilter<T1, T2>(this AsyncHandler<T1, IEnumerable<T2>> handler, Filter<T2> filter) {
    return handler.OutputTransform(filter);
  }

}