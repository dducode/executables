using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables.Enumeration;

public static partial class ExecutableEnumerableExtensions {

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ExecutableEnumerable<T1, T2>(query, enumerable);
  }

  [Pure]
  public static ExecutableList<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, List<T1> list) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(list, nameof(list));
    return new ExecutableList<T1, T2>(query, list);
  }

  [Pure]
  public static ExecutableArray<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, T1[] array) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(array, nameof(array));
    return new ExecutableArray<T1, T2>(query, array);
  }

  [Pure]
  public static ExecutableHashSet<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, HashSet<T1> hashSet) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(hashSet, nameof(hashSet));
    return new ExecutableHashSet<T1, T2>(query, hashSet);
  }

  [Pure]
  public static ExecutableQueue<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, Queue<T1> queue) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(queue, nameof(queue));
    return new ExecutableQueue<T1, T2>(query, queue);
  }

  [Pure]
  public static ExecutableStack<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, Stack<T1> stack) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(stack, nameof(stack));
    return new ExecutableStack<T1, T2>(query, stack);
  }

#if !NETFRAMEWORK
  [Pure]
  public static ExecutableSpan<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, Span<T1> span) {
    query.ThrowIfNullReference();
    return new ExecutableSpan<T1, T2>(query, span);
  }

  [Pure]
  public static ExecutableReadOnlySpan<T1, T2> ForEach<T1, T2>(this IQuery<T1, T2> query, ReadOnlySpan<T1> span) {
    query.ThrowIfNullReference();
    return new ExecutableReadOnlySpan<T1, T2>(query, span);
  }
#endif

}