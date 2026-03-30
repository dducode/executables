using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Enumeration;

public static class ManyExecutableEnumerableExtensions {

  /// <summary>
  /// Lazily applies a query that returns sequences and flattens the combined results.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableEnumerable<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, IEnumerable<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableEnumerable<T1, T2>(query, enumerable);
  }

  /// <summary>
  /// Lazily applies a query that returns lists and flattens the combined results.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableList<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, List<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableList<T1, T2>(query, enumerable);
  }

  /// <summary>
  /// Lazily applies a query that returns arrays and flattens the combined results.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableArray<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, T2[]> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableArray<T1, T2>(query, enumerable);
  }

  /// <summary>
  /// Lazily applies a query that returns hash sets and flattens the combined results.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableHashSet<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, HashSet<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableHashSet<T1, T2>(query, enumerable);
  }

  /// <summary>
  /// Lazily applies a query that returns queues and flattens the combined results.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableQueue<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, Queue<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableQueue<T1, T2>(query, enumerable);
  }

  /// <summary>
  /// Lazily applies a query that returns stacks and flattens the combined results.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableStack<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, Stack<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableStack<T1, T2>(query, enumerable);
  }

}