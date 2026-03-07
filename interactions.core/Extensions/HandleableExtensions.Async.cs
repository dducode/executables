using System.Diagnostics.Contracts;
using Interactions.Core.Handlers;

namespace Interactions.Core.Extensions;

public static partial class HandleableExtensions {

  [Pure]
  public static AsyncHandleable<T1, T2> Merge<T1, T2>(this AsyncHandleable<T1, T2> first, AsyncHandleable<T1, T2> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return new AsyncMergedHandleable<T1, T2>(first, second);
  }

  public static IDisposable Handle<T1, T2>(this AsyncHandleable<T1, T2> handleable, AsyncFunc<T1, T2> handler) {
    return handleable.Handle(AsyncHandler.FromMethod(handler));
  }

  public static IDisposable Handle<T>(this AsyncHandleable<Unit, T> handleable, AsyncFunc<T> handler) {
    return handleable.Handle(AsyncHandler.FromMethod(handler));
  }

  public static IDisposable Handle<T>(this AsyncHandleable<T, Unit> handleable, AsyncAction<T> handler) {
    return handleable.Handle(AsyncHandler.FromMethod(handler));
  }

  public static IDisposable Handle(this AsyncHandleable<Unit, Unit> handleable, AsyncAction handler) {
    return handleable.Handle(AsyncHandler.FromMethod(handler));
  }

  public static IDisposable Handle<T1, T2>(this AsyncHandleable<T1, T2> handleable, Func<T1, T2> handler) {
    return handleable.Handle(Handler.FromMethod(handler).ToAsyncHandler());
  }

  public static IDisposable Handle<T>(this AsyncHandleable<Unit, T> handleable, Func<T> handler) {
    return handleable.Handle(Handler.FromMethod(handler).ToAsyncHandler());
  }

  public static IDisposable Handle<T>(this AsyncHandleable<T, Unit> handleable, Action<T> handler) {
    return handleable.Handle(Handler.FromMethod(handler).ToAsyncHandler());
  }

  public static IDisposable Handle(this AsyncHandleable<Unit, Unit> handleable, Action handler) {
    return handleable.Handle(Handler.FromMethod(handler).ToAsyncHandler());
  }

}