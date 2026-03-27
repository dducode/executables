#if !NETFRAMEWORK
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Interactions.Internal;

namespace Interactions.Enumeration;

public static partial class ExecutableEnumerableExtensions {

  /// <summary>
  /// Lazily applies an asynchronous query to each element of an asynchronous sequence.
  /// </summary>
  /// <param name="query">Query applied to each source item.</param>
  /// <param name="enumerable">Source asynchronous sequence.</param>
  /// <returns>Lazy asynchronous enumerable that executes <paramref name="query"/> during iteration.</returns>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static AsyncExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IAsyncQuery<T1, T2> query, IAsyncEnumerable<T1> enumerable) {
    query.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new AsyncExecutableEnumerable<T1, T2>(query, enumerable);
  }

}
#endif