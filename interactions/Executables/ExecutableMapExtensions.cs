using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Maps;
using Interactions.Transformation;
using Interactions.Transformation.Filtering;
using Interactions.Transformation.Parsing;

namespace Interactions.Executables;

public static partial class ExecutableMapExtensions {

  [Pure]
  public static IExecutable<T2, T2> Map<T1, T2>(this IExecutable<T1, T1> executable, SymmetricTransformer<T2, T1> transformer) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(transformer, nameof(transformer));
    return new SymmetricExecutableMap<T2, T1>(new SymmetricMap<T2, T1>(transformer), executable);
  }

  [Pure]
  public static IExecutable<string, string> Parse<T1, T2>(this IExecutable<T1, T2> executable, Parser<T1> parser) {
    return executable.Map(parser, ToStringTransformer<T2>.Instance);
  }

  [Pure]
  public static IExecutable<string, T2> InParse<T1, T2>(this IExecutable<T1, T2> executable, Parser<T1> parser) {
    return executable.InMap(parser);
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T2>> Filter<T1, T2>(
    this IExecutable<IEnumerable<T1>, IEnumerable<T2>> executable,
    Filter<T1> incoming,
    Filter<T2> outgoing) {
    return executable.Map(incoming, outgoing);
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, T2> InFilter<T1, T2>(this IExecutable<IEnumerable<T1>, T2> executable, Filter<T1> filter) {
    return executable.InMap(filter);
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> OutFilter<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, Filter<T2> filter) {
    return executable.OutMap(filter);
  }

}