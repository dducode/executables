using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableHandler<T1, T2>(executable);
  }

  [Pure]
  public static IAsyncQuery<T1, T2> AsQuery<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableQuery<T1, T2>(executable);
  }

  [Pure]
  public static IAsyncCommand<T> AsCommand<T>(this IAsyncExecutable<T, bool> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableCommand<T>(executable);
  }

}