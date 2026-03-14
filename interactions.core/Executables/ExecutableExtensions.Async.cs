using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  public static ValueTask<T> Execute<T>(this IAsyncExecutor<Unit, T> executor, CancellationToken token = default) {
    return executor.Execute(default, token);
  }

  public static async ValueTask Execute(this IAsyncExecutor<Unit, Unit> executor, CancellationToken token = default) {
    await executor.Execute(default, token);
  }

  public static async ValueTask<Result<T2>> TryExecute<T1, T2>(this IAsyncExecutor<T1, T2> executor, T1 input, CancellationToken token = default) {
    try {
      return await executor.Execute(input, token);
    }
    catch (OperationCanceledException e) {
      return Result<T2>.FromException(e);
    }
  }

  public static async ValueTask<Result<T>> TryExecute<T>(this IAsyncExecutor<Unit, T> executor, CancellationToken token = default) {
    return await executor.TryExecute(default, token);
  }

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