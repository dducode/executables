using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;
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

  [Pure]
  public static IExecutable<T1, T3> Next<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new CompositeExecutable<T1, T2, T3>(first, second);
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

}