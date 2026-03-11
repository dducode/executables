using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables.Enumeration;

public static partial class ExecutableEnumerableExtensions {

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, IEnumerable<T1> enumerable) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ExecutableEnumerable<T1, T2>(executable, enumerable);
  }

  [Pure]
  public static ExecutableList<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, List<T1> list) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(list, nameof(list));
    return new ExecutableList<T1, T2>(executable, list);
  }

  [Pure]
  public static ExecutableHashSet<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, HashSet<T1> hashSet) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(hashSet, nameof(hashSet));
    return new ExecutableHashSet<T1, T2>(executable, hashSet);
  }

  [Pure]
  public static ExecutableQueue<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, Queue<T1> queue) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(queue, nameof(queue));
    return new ExecutableQueue<T1, T2>(executable, queue);
  }

  [Pure]
  public static ExecutableStack<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, Stack<T1> stack) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(stack, nameof(stack));
    return new ExecutableStack<T1, T2>(executable, stack);
  }

#if !NETFRAMEWORK
  [Pure]
  public static ExecutableSpan<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, Span<T1> span) {
    executable.ThrowIfNullReference();
    return new ExecutableSpan<T1, T2>(executable, span);
  }

  [Pure]
  public static ExecutableReadOnlySpan<T1, T2> ForEach<T1, T2>(this IExecutable<T1, T2> executable, ReadOnlySpan<T1> span) {
    executable.ThrowIfNullReference();
    return new ExecutableReadOnlySpan<T1, T2>(executable, span);
  }
#endif

}