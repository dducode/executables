using System.Diagnostics.Contracts;

namespace Interactions.Core.Extensions;

public static class ExecutableExtensions {

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
  public static IAsyncExecutable<T1, T2> ToAsyncExecutable<T1, T2>(this IExecutable<T1, T2> inner) {
    ExceptionsHelper.ThrowIfNullReference(inner);
    return new AsyncProxyExecutable<T1, T2>(inner);
  }

}