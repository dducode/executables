using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  public static T Execute<T>(this IExecutor<Unit, T> executor) {
    return executor.Execute(default);
  }

  public static void Execute(this IExecutor<Unit, Unit> executor) {
    executor.Execute(default);
  }

  public static Result<T2> TryExecute<T1, T2>(this IExecutor<T1, T2> executor, T1 input) {
    try {
      return executor.Execute(input);
    }
    catch (MissingHandlerException e) {
      return Result<T2>.FromException(e);
    }
  }

  public static Result<T> TryExecute<T>(this IExecutor<Unit, T> executor) {
    return executor.TryExecute(default);
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> ToAsyncExecutable<T1, T2>(this IExecutable<T1, T2> inner) {
    inner.ThrowIfNullReference();
    return new AsyncProxyExecutable<T1, T2>(inner);
  }

  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableHandler<T1, T2>(executable);
  }

  [Pure]
  public static IQuery<T1, T2> AsQuery<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableQuery<T1, T2>(executable);
  }

  [Pure]
  public static ICommand<T> AsCommand<T>(this IExecutable<T, bool> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableCommand<T>(executable);
  }

}