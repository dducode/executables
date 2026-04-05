using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Enumeration;

public static class ManyExecutableEnumerableExtensions {

  /// <summary>
  /// Creates a lazy flattened projection by executing an executor that returns sequences.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of flattened result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable produced from sequence results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableEnumerable<T1, T2> ForEachMany<T1, T2>(this IExecutor<T1, IEnumerable<T2>> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableEnumerable<T1, T2>(executor, enumerable);
  }

  /// <summary>
  /// Creates a lazy flattened projection by executing an executor that returns lists.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of flattened result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable produced from list results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableList<T1, T2> ForEachMany<T1, T2>(this IExecutor<T1, List<T2>> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableList<T1, T2>(executor, enumerable);
  }

  /// <summary>
  /// Creates a lazy flattened projection by executing an executor that returns arrays.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of flattened result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable produced from array results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableArray<T1, T2> ForEachMany<T1, T2>(this IExecutor<T1, T2[]> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableArray<T1, T2>(executor, enumerable);
  }

  /// <summary>
  /// Creates a lazy flattened projection by executing an executor that returns hash sets.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of flattened result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable produced from hash-set results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableHashSet<T1, T2> ForEachMany<T1, T2>(this IExecutor<T1, HashSet<T2>> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableHashSet<T1, T2>(executor, enumerable);
  }

  /// <summary>
  /// Creates a lazy flattened projection by executing an executor that returns queues.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of flattened result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable produced from queue results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableQueue<T1, T2> ForEachMany<T1, T2>(this IExecutor<T1, Queue<T2>> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableQueue<T1, T2>(executor, enumerable);
  }

  /// <summary>
  /// Creates a lazy flattened projection by executing an executor that returns stacks.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of flattened result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy flattened enumerable produced from stack results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableStack<T1, T2> ForEachMany<T1, T2>(this IExecutor<T1, Stack<T2>> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableStack<T1, T2>(executor, enumerable);
  }

}