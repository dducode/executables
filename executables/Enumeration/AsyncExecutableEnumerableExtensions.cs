#if !NETFRAMEWORK
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Executables.Internal;

namespace Executables.Enumeration;

public static class AsyncExecutableEnumerableExtensions {

  /// <summary>
  /// Creates a lazy async projection that executes an async executor for each source element.
  /// </summary>
  /// <typeparam name="T1">Type of source sequence elements.</typeparam>
  /// <typeparam name="T2">Type of produced result elements.</typeparam>
  /// <param name="executor">Executor applied to each source element during enumeration.</param>
  /// <param name="enumerable">Source asynchronous sequence.</param>
  /// <returns>Lazy asynchronous enumerable that executes <paramref name="executor"/> while iterating.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static AsyncExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IAsyncExecutor<T1, T2> executor, IAsyncEnumerable<T1> enumerable) {
    executor.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new AsyncExecutableEnumerable<T1, T2>(executor, enumerable);
  }

}
#endif