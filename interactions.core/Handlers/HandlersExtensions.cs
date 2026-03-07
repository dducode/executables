using System.Diagnostics.Contracts;

namespace Interactions.Core.Handlers;

public static class HandlersExtensions {

  [Pure]
  public static AsyncHandler<T1, T2> ToAsyncHandler<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return new AsyncProxyHandler<T1, T2>(handler);
  }

  [Pure]
  public static AsyncHandler<T1, T3> Then<T1, T2, T3>(this AsyncHandler<T1, T2> handler, AsyncHandler<T2, T3> nextHandler) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new AsyncCompositeHandler<T1, T2, T3>(handler, nextHandler);
  }

  [Pure]
  public static AsyncHandler<T1, T3> Then<T1, T2, T3>(this AsyncHandler<T1, T2> handler, Handler<T2, T3> nextHandler) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new AsyncCompositeHandler<T1, T2, T3>(handler, nextHandler.ToAsyncHandler());
  }

  [Pure]
  public static AsyncHandler<T1, T3> Then<T1, T2, T3>(this AsyncHandler<T1, T2> handler, AsyncFunc<T2, T3> nextHandler) {
    return handler.Then(AsyncHandler.Create(nextHandler));
  }

  [Pure]
  public static AsyncHandler<T1, T3> Then<T1, T2, T3>(this AsyncHandler<T1, T2> handler, Func<T2, T3> nextHandler) {
    return handler.Then(Handler.Create(nextHandler));
  }

  [Pure]
  public static Handler<T1, T3> Then<T1, T2, T3>(this Handler<T1, T2> handler, Handler<T2, T3> nextHandler) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new CompositeHandler<T1, T2, T3>(handler, nextHandler);
  }

  [Pure]
  public static AsyncHandler<T1, T3> Then<T1, T2, T3>(this Handler<T1, T2> handler, AsyncHandler<T2, T3> nextHandler) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(nextHandler, nameof(nextHandler));
    return new AsyncCompositeHandler<T1, T2, T3>(handler.ToAsyncHandler(), nextHandler);
  }

  [Pure]
  public static Handler<T1, T3> Then<T1, T2, T3>(this Handler<T1, T2> handler, Func<T2, T3> nextHandler) {
    return handler.Then(Handler.Create(nextHandler));
  }

  [Pure]
  public static AsyncHandler<T1, T3> Then<T1, T2, T3>(this Handler<T1, T2> handler, AsyncFunc<T2, T3> nextHandler) {
    return handler.Then(AsyncHandler.Create(nextHandler));
  }

  [Pure]
  public static AsyncHandler<T1, T2> OnDispose<T1, T2>(this AsyncHandler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AsyncAnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

  [Pure]
  public static Handler<T1, T2> OnDispose<T1, T2>(this Handler<T1, T2> handler, Action dispose) {
    handler.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(dispose, nameof(dispose));
    return new AnonymousDisposeHandler<T1, T2>(handler, dispose);
  }

}