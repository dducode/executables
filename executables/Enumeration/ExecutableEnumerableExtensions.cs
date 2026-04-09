using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Enumeration;

/// <summary>
/// Extension methods for creating executable-backed enumerable projections.
/// </summary>
public static class ExecutableEnumerableExtensions {

  /// <summary>
  /// Creates a lazy projection that executes an executor for each source element.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, IEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ExecutableEnumerable<T1, T2>(executor, enumerable);
  }

  /// <summary>
  /// Creates a lazy projection that executes an executor for each list element.
  /// </summary>
  /// <typeparam name="T1">Type of source list elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="list">Source list.</param>
  /// <returns>Lazy list-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ExecutableList<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, List<T1> list) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(list, nameof(list));
    return new ExecutableList<T1, T2>(executor, list);
  }

  /// <summary>
  /// Creates a lazy projection that executes an executor for each array element.
  /// </summary>
  /// <typeparam name="T1">Type of source array elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="array">Source array.</param>
  /// <returns>Lazy array-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ExecutableArray<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, T1[] array) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(array, nameof(array));
    return new ExecutableArray<T1, T2>(executor, array);
  }

  /// <summary>
  /// Creates a lazy projection that executes an executor for each hash-set element.
  /// </summary>
  /// <typeparam name="T1">Type of source hash-set elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="hashSet">Source hash set.</param>
  /// <returns>Lazy hash-set-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="hashSet"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ExecutableHashSet<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, HashSet<T1> hashSet) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(hashSet, nameof(hashSet));
    return new ExecutableHashSet<T1, T2>(executor, hashSet);
  }

  /// <summary>
  /// Creates a lazy projection that executes an executor for each queue element.
  /// </summary>
  /// <typeparam name="T1">Type of source queue elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="queue">Source queue.</param>
  /// <returns>Lazy queue-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ExecutableQueue<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, Queue<T1> queue) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(queue, nameof(queue));
    return new ExecutableQueue<T1, T2>(executor, queue);
  }

  /// <summary>
  /// Creates a lazy projection that executes an executor for each stack element.
  /// </summary>
  /// <typeparam name="T1">Type of source stack elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="stack">Source stack.</param>
  /// <returns>Lazy stack-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
  [Pure]
  public static ExecutableStack<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, Stack<T1> stack) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(stack, nameof(stack));
    return new ExecutableStack<T1, T2>(executor, stack);
  }

#if !NETFRAMEWORK
  /// <summary>
  /// Creates a lazy projection that executes an executor for each mutable span element.
  /// </summary>
  /// <typeparam name="T1">Type of source span elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="span">Source span.</param>
  /// <returns>Lazy span-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  [Pure]
  public static ExecutableSpan<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, Span<T1> span) {
    executor.ThrowIfNullReference();
    return new ExecutableSpan<T1, T2>(executor, span);
  }

  /// <summary>
  /// Creates a lazy projection that executes an executor for each read-only span element.
  /// </summary>
  /// <typeparam name="T1">Type of source span elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="span">Source span.</param>
  /// <returns>Lazy read-only-span-backed enumerable that executes <paramref name="executor"/> while iterating.</returns>
  [Pure]
  public static ExecutableReadOnlySpan<T1, T2> ForEach<T1, T2>(this IExecutor<T1, T2> executor, ReadOnlySpan<T1> span) {
    executor.ThrowIfNullReference();
    return new ExecutableReadOnlySpan<T1, T2>(executor, span);
  }
#endif

}