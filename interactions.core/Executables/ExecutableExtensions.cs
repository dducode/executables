using System.Diagnostics.Contracts;
using Interactions.Core.Internal;

namespace Interactions.Core.Executables;

public static partial class ExecutableExtensions {

  public static T Execute<T>(this IExecutable<Unit, T> executable) {
    return executable.Execute(default);
  }

  public static Result<T2> TryExecute<T1, T2>(this IExecutable<T1, T2> executable, T1 input) {
    try {
      return executable.Execute(input);
    }
    catch (MissingHandlerException e) {
      return Result<T2>.FromException(e);
    }
  }

  public static Result<T> TryExecute<T>(this IExecutable<Unit, T> executable) {
    return executable.TryExecute(default);
  }

  [Pure]
  public static IAsyncExecutable<T1, T2> ToAsyncExecutable<T1, T2>(this IExecutable<T1, T2> inner) {
    inner.ThrowIfNullReference();
    return new AsyncProxyExecutable<T1, T2>(inner);
  }

  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableHandlerWrapper<T1, T2>(executable);
  }

}