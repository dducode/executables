using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Filtering;

namespace Interactions.Executables;

public static partial class ExecutableMapExtensions {

  [Pure]
  public static IExecutable<string, string> Parse<T1, T2>(this IExecutable<T1, T2> executable, IExecutable<string, T1> parser) {
    return executable.Map(parser, Executable.Create((T2 t2) => t2.ToString()));
  }

  [Pure]
  public static IExecutable<string, string> Parse<T1, T2>(this IExecutable<T1, T2> executable, Func<string, T1> parser) {
    return executable.Parse(Executable.Create(parser));
  }

  [Pure]
  public static IExecutable<string, T2> InParse<T1, T2>(this IExecutable<T1, T2> executable, IExecutable<string, T1> parser) {
    return executable.InMap(parser);
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T2>> Filter<T1, T2>(
    this IExecutable<IEnumerable<T1>, IEnumerable<T2>> executable,
    IFilter<T1> incoming,
    IFilter<T2> outgoing) {
    return executable.Map(incoming, outgoing);
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, T2> InFilter<T1, T2>(this IExecutable<IEnumerable<T1>, T2> executable, IFilter<T1> filter) {
    return executable.InMap(filter);
  }

  [Pure]
  public static IExecutable<T1, IEnumerable<T2>> OutFilter<T1, T2>(this IExecutable<T1, IEnumerable<T2>> executable, IFilter<T2> filter) {
    return executable.OutMap(filter);
  }

}