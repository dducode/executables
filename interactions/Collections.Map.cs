using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions;

public static partial class Collections {

  [Pure]
  public static IExecutable<IEnumerable<T>, T> Aggregate<T>(Func<T, T, T> accumulate) {
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return Executable.Create((IEnumerable<T> enumerable) => enumerable.Aggregate(accumulate));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, T2> Aggregate<T1, T2>(Func<T2> seed, Func<T2, T1, T2> accumulate) {
    ExceptionsHelper.ThrowIfNull(seed, nameof(seed));
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return Executable.Create((IEnumerable<T1> enumerable) => enumerable.Aggregate(seed(), accumulate));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T2>> Select<T1, T2>(Func<T1, T2> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return Executable.Create((IEnumerable<T1> enumerable) => enumerable.Select(selection));
  }

  [Pure]
  public static IExecutable<IEnumerable<T1>, IEnumerable<T2>> SelectMany<T1, T2>(Func<T1, IEnumerable<T2>> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return Executable.Create((IEnumerable<T1> enumerable) => enumerable.SelectMany(selection));
  }

  [Pure]
  public static IExecutable<IEnumerable<T>, T> First<T>(Func<T, bool> predicate = null) {
    return Executable.Create((IEnumerable<T> enumerable) => predicate != null ? enumerable.First(predicate) : enumerable.First());
  }

  [Pure]
  public static IExecutable<string, string[]> Split(string separator) {
    ExceptionsHelper.ThrowIfNull(separator, nameof(separator));
    char[] separators = separator.ToCharArray();
    return Executable.Create((string s) => s.Split(separators, StringSplitOptions.RemoveEmptyEntries));
  }

}