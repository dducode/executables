using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables.Enumeration;

public static partial class ExecutableEnumerableExtensions {

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableEnumerable<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, IEnumerable<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableEnumerable<T1, T2>(query, enumerable);
  }

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableList<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, List<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableList<T1, T2>(query, enumerable);
  }

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableArray<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, T2[]> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableArray<T1, T2>(query, enumerable);
  }

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableHashSet<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, HashSet<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableHashSet<T1, T2>(query, enumerable);
  }

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableQueue<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, Queue<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableQueue<T1, T2>(query, enumerable);
  }

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static ManyExecutableStack<T1, T2> ForEachMany<T1, T2>(this IQuery<T1, Stack<T2>> query, IEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new ManyExecutableStack<T1, T2>(query, enumerable);
  }

}