using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Enumeration;

public static class ExecutableEnumerableExtensions {

  /// <summary>
  /// Lazily applies a query to each element of an enumerable sequence.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source sequence.</param>
  /// <returns>Lazy enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ExecutableEnumerable<T1, T2>(query, enumerable);
  }

  /// <summary>
  /// Lazily applies a query to each element of a list.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="list">Source list.</param>
  /// <returns>Lazy enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableList<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, List<T1> list) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(list, nameof(list));
    return new ExecutableList<T1, T2>(query, list);
  }

  /// <summary>
  /// Lazily applies a query to each element of an array.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="array">Source array.</param>
  /// <returns>Lazy enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableArray<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, T1[] array) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(array, nameof(array));
    return new ExecutableArray<T1, T2>(query, array);
  }

  /// <summary>
  /// Lazily applies a query to each element of a hash set.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="hashSet">Source hash set.</param>
  /// <returns>Lazy enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableHashSet<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, HashSet<T1> hashSet) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(hashSet, nameof(hashSet));
    return new ExecutableHashSet<T1, T2>(query, hashSet);
  }

  /// <summary>
  /// Lazily applies a query to each element of a queue.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="queue">Source queue.</param>
  /// <returns>Lazy enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableQueue<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, Queue<T1> queue) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(queue, nameof(queue));
    return new ExecutableQueue<T1, T2>(query, queue);
  }

  /// <summary>
  /// Lazily applies a query to each element of a stack.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="stack">Source stack.</param>
  /// <returns>Lazy enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableStack<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, Stack<T1> stack) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(stack, nameof(stack));
    return new ExecutableStack<T1, T2>(query, stack);
  }

#if !NETFRAMEWORK
  /// <summary>
  /// Lazily applies a query to each element of a mutable span.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="span">Source span.</param>
  /// <returns>Lazy span-based enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableSpan<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, Span<T1> span) {
    query.ThrowIfNullReference();
    return new ExecutableSpan<T1, T2>(query, span);
  }

  /// <summary>
  /// Lazily applies a query to each element of a read-only span.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="span">Source span.</param>
  /// <returns>Lazy span-based enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  public static ExecutableReadOnlySpan<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, ReadOnlySpan<T1> span) {
    query.ThrowIfNullReference();
    return new ExecutableReadOnlySpan<T1, T2>(query, span);
  }
#endif

}