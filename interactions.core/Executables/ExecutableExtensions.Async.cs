using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  public static ValueTask<T> Execute<T>(this IAsyncExecutable<Unit, T> executable, CancellationToken token = default) {
    return executable.Execute(default, token);
  }

  public static async ValueTask Execute(this IAsyncExecutable<Unit, Unit> executable, CancellationToken token = default) {
    await executable.Execute(default, token);
  }

  public static async ValueTask<Result<T2>> TryExecute<T1, T2>(this IAsyncExecutable<T1, T2> executable, T1 input, CancellationToken token = default) {
    try {
      return await executable.Execute(input, token);
    }
    catch (Exception e) when (e is MissingHandlerException or OperationCanceledException) {
      return Result<T2>.FromException(e);
    }
  }

  public static async ValueTask<Result<T>> TryExecute<T>(this IAsyncExecutable<Unit, T> executable, CancellationToken token = default) {
    return await executable.TryExecute(default, token);
  }

  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableHandlerWrapper<T1, T2>(executable);
  }

}