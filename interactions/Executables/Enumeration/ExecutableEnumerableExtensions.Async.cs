#if !NETFRAMEWORK
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Executables.Enumeration;

public static partial class ExecutableEnumerableExtensions {

  [Pure]
  [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
  public static AsyncExecutableEnumerable<T1, T2> ForEach<T1, T2>(this IAsyncExecutable<T1, T2> executable, IAsyncEnumerable<T1> enumerable) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(enumerable, nameof(enumerable));
    return new AsyncExecutableEnumerable<T1, T2>(executable, enumerable);
  }

}
#endif