using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core;

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
    inner.ThrowIfNullReference();
    return new AsyncProxyExecutable<T1, T2>(inner);
  }

  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableHandlerWrapper<T1, T2>(executable);
  }

  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableHandlerWrapper<T1, T2>(executable);
  }

  [Pure]
  public static IExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeExecutable<T1, T2, T3>(first, second);
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
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    return first.ToAsyncExecutable().Next(second);
  }

  [Pure]
  public static IExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Next(Executable.Create(next));
  }

  [Pure]
  public static IAsyncExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.ToAsyncExecutable().Next(AsyncExecutable.Create(next));
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