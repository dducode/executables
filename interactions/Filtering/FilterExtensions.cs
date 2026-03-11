using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Executables;

namespace Interactions.Filtering;

public static class FilterExtensions {

  [Pure]
  public static IFilter<T> Where<T>(this IFilter<T> filter, Validator<T> validator) {
    return filter.Then(Executable.Create((IEnumerable<T> enumerable) => enumerable.Where(validator.IsValid))).AsFilter();
  }

  [Pure]
  public static IFilter<T> Where<T>(this IFilter<T> filter, Func<T, bool> predicate) {
    return filter.Where(Validator.Create(predicate, string.Empty));
  }

  [Pure]
  public static IFilter<T> Distinct<T>(
    this IFilter<T> filter,
    IEqualityComparer<T> equalityComparer = null) {
    return filter.Then(Executable.Create((IEnumerable<T> enumerable) => enumerable.Distinct(equalityComparer))).AsFilter();
  }

  [Pure]
  public static IFilter<T> Skip<T>(this IFilter<T> filter, int skipCount) {
    return filter.Then(Executable.Create((IEnumerable<T> enumerable) => enumerable.Skip(skipCount))).AsFilter();
  }

  [Pure]
  public static IFilter<T> Take<T>(this IFilter<T> filter, int takeCount) {
    return filter.Then(Executable.Create((IEnumerable<T> enumerable) => enumerable.Take(takeCount))).AsFilter();
  }

  [Pure]
  public static IExecutable<List<T>, List<T>> ListFilter<T>(
    this IExecutable<List<T>, List<T>> executable,
    IFilter<T> incoming,
    IFilter<T> outgoing) {
    return executable.Map(Executable.Create((IEnumerable<T> enumerable) => enumerable.ToList()), Executable.Create((List<T> list) => list.AsEnumerable()))
      .Filter(incoming, outgoing)
      .Map(Executable.Create((List<T> list) => list.AsEnumerable()), Executable.Create((IEnumerable<T> enumerable) => enumerable.ToList()));
  }

  [Pure]
  public static IExecutable<List<T1>, T2> InputListFilter<T1, T2>(this IExecutable<List<T1>, T2> executable, IFilter<T1> incoming) {
    return executable.InMap(Executable.Create((IEnumerable<T1> enumerable) => enumerable.ToList()))
      .InFilter(incoming)
      .InMap(Executable.Create((List<T1> list) => list.AsEnumerable()));
  }

  [Pure]
  public static IExecutable<T1, List<T2>> OutputListFilter<T1, T2>(this IExecutable<T1, List<T2>> executable, IFilter<T2> outgoing) {
    return executable.OutMap(Executable.Create((List<T2> list) => list.AsEnumerable()))
      .OutFilter(outgoing)
      .OutMap(Executable.Create((IEnumerable<T2> enumerable) => enumerable.ToList()));
  }

  [Pure]
  public static IExecutable<T[], T[]> ArrayFilter<T>(
    this IExecutable<T[], T[]> executable,
    IFilter<T> incoming,
    IFilter<T> outgoing) {
    return executable.Map(Executable.Create((IEnumerable<T> enumerable) => enumerable.ToArray()), Executable.Create((T[] array) => array.AsEnumerable()))
      .Filter(incoming, outgoing)
      .Map(Executable.Create((T[] array) => array.AsEnumerable()), Executable.Create((IEnumerable<T> enumerable) => enumerable.ToArray()));
  }

  [Pure]
  public static IExecutable<T1[], T2> InputArrayFilter<T1, T2>(this IExecutable<T1[], T2> executable, IFilter<T1> incoming) {
    return executable.InMap(Executable.Create((IEnumerable<T1> enumerable) => enumerable.ToArray()))
      .InFilter(incoming)
      .InMap(Executable.Create((T1[] array) => array.AsEnumerable()));
  }

  [Pure]
  public static IExecutable<T1, T2[]> OutputArrayFilter<T1, T2>(this IExecutable<T1, T2[]> executable, IFilter<T2> outgoing) {
    return executable.OutMap(Executable.Create((T2[] array) => array.AsEnumerable()))
      .OutFilter(outgoing)
      .OutMap(Executable.Create((IEnumerable<T2> enumerable) => enumerable.ToArray()));
  }

}