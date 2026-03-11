using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables;

public static class ExecutableSelectionsExtensions {

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T3>> Select<T1, T2, T3>(
    this IExecutable<IEnumerable<T1>, IEnumerable<T2>> transformer,
    Func<T2, T3> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return transformer.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.Select(selection)));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T3>> SelectMany<T1, T2, T3>(
    this IExecutable<IEnumerable<T1>, IEnumerable<T2>> transformer,
    Func<T2, IEnumerable<T3>> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return transformer.Then(Executable.Create((IEnumerable<T2> enumerable) => enumerable.SelectMany(selection)));
  }

}