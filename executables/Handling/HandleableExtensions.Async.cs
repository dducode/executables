using System.Diagnostics.Contracts;
using Executables.Core.Handleables;
using Executables.Internal;

namespace Executables.Handling;

public static partial class HandleableExtensions {

  [Pure]
  public static IAsyncHandleable<T1, T2, THandler> Merge<T1, T2, THandler>(
    this IAsyncHandleable<T1, T2, THandler> first,
    IAsyncHandleable<T1, T2, THandler> second) where THandler : AsyncHandler<T1, T2> {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncCompositeHandleable<T1, T2, THandler>(first, second);
  }

  public static IDisposable Handle<T1, T2>(this IAsyncHandleable<T1, T2> handleable, AsyncFunc<T1, T2> handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  public static IDisposable Handle<T>(this IAsyncHandleable<Unit, T> handleable, AsyncFunc<T> handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  public static IDisposable Handle<T>(this IAsyncHandleable<T, Unit> handleable, AsyncAction<T> handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  public static IDisposable Handle(this IAsyncHandleable<Unit, Unit> handleable, AsyncAction handler) {
    return handleable.Handle(AsyncExecutable.Create(handler).AsHandler());
  }

  public static IDisposable Handle<T1, T2>(this IAsyncHandleable<T1, T2> handleable, Func<T1, T2> handler) {
    return handleable.Handle(Executable.Create(handler).ToAsyncExecutable().AsHandler());
  }

  public static IDisposable Handle<T>(this IAsyncHandleable<Unit, T> handleable, Func<T> handler) {
    return handleable.Handle(Executable.Create(handler).ToAsyncExecutable().AsHandler());
  }

  public static IDisposable Handle<T>(this IAsyncHandleable<T, Unit> handleable, Action<T> handler) {
    return handleable.Handle(Executable.Create(handler).ToAsyncExecutable().AsHandler());
  }

  public static IDisposable Handle(this IAsyncHandleable<Unit, Unit> handleable, Action handler) {
    return handleable.Handle(Executable.Create(handler).ToAsyncExecutable().AsHandler());
  }

}