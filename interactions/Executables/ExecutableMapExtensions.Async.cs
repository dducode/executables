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
  public static IAsyncExecutable<T2, T2> Map<T1, T2>(this IAsyncExecutable<T1, T1> executable, SymmetricTransformer<T2, T1> transformer) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(transformer, nameof(transformer));
    return new AsyncSymmetricExecutableMap<T2, T1>(new AsyncSymmetricMap<T2, T1>(transformer), executable);
  }

  [Pure]
  public static IAsyncExecutable<string, string> Parse<T1, T2>(this IAsyncExecutable<T1, T2> executable, Parser<T1> parser) {
    return executable.Map(parser, ToStringTransformer<T2>.Instance);
  }

  [Pure]
  public static IAsyncExecutable<string, T2> InParse<T1, T2>(this IAsyncExecutable<T1, T2> executable, Parser<T1> parser) {
    return executable.InMap(parser);
  }

  [Pure]
  public static IAsyncExecutable<IEnumerable<T1>, IEnumerable<T2>> Filter<T1, T2>(
    this IAsyncExecutable<IEnumerable<T1>, IEnumerable<T2>> executable,
    Filter<T1> incoming,
    Filter<T2> outgoing) {
    return executable.Map(incoming, outgoing);
  }

  [Pure]
  public static IAsyncExecutable<IEnumerable<T1>, T2> InFilter<T1, T2>(this IAsyncExecutable<IEnumerable<T1>, T2> executable, Filter<T1> filter) {
    return executable.InMap(filter);
  }

  [Pure]
  public static IAsyncExecutable<T1, IEnumerable<T2>> OutFilter<T1, T2>(this IAsyncExecutable<T1, IEnumerable<T2>> executable, Filter<T2> filter) {
    return executable.OutMap(filter);
  }

}