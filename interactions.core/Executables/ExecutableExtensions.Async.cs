using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  public static ValueTask<T> Execute<T>(this IAsyncExecutable<Unit, T> executable, CancellationToken token = default) {
    return executable.Execute(default, token);
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

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeExecutable<T1, T2, T3>(first, second);
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    return first.Next(second.ToAsyncExecutable());
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Next(Executable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.Next(AsyncExecutable.Create(next));
  }

}